using System;
using System.Configuration;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Service.EngineToServer;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Service
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class EngineConfigurationBus : IEngineConfigurationBus
    {
        private readonly IObjectSerializer _objectSerializer;
        private readonly IAuthenticationKeyProvider _authenticationKeyProvider;
        private readonly IAuthenticationRequestEncryptor _authenticationRequestEncryptor;
        private readonly IAuthenticatedCommunicationRequestEncryptor _authenticatedCommunicationRequestEncryptor;
        private readonly IEngineToServerCommunicationWcfService _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticationKeyProvider">The authentication key provider.</param>
        /// <param name="authenticationRequestEncryptor">The authentication request encryptor.</param>
        /// <param name="authenticatedCommunicationRequestEncryptor">The authenticated communication request encryptor.</param>
        public EngineConfigurationBus(IEngineToServerConnection engineToServerConnection,
            IObjectSerializer objectSerializer,
            IAuthenticationKeyProvider authenticationKeyProvider,
            IAuthenticationRequestEncryptor authenticationRequestEncryptor,
            IAuthenticatedCommunicationRequestEncryptor authenticatedCommunicationRequestEncryptor)
        {

            _objectSerializer = objectSerializer;
            _authenticationKeyProvider = authenticationKeyProvider;
            _authenticationRequestEncryptor = authenticationRequestEncryptor;
            _authenticatedCommunicationRequestEncryptor = authenticatedCommunicationRequestEncryptor;
            _channel = engineToServerConnection.OpenChannel();
        }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            #region Pre-auth
            var preAuthResponseBytes = _channel.PreAuthenticate();

            var preAuthentication = _objectSerializer.ToObject<EnginePreAuthenticationResponse>(preAuthResponseBytes);

            if (!preAuthentication.Success)
            {
                throw new ConfigurationErrorsException(preAuthentication.ErrorMessage);
            }
            #endregion

            #region Auth
            var serverPublicKey = new PublicKey(Convert.FromBase64String(preAuthentication.PublicKeyForReply));

            var authRequestString = _objectSerializer.ToBytes(request);

            var authResponseBytes = _channel.Authenticate(new AsymmetricEnvelope
            {
                KeyHash = serverPublicKey.GetHashString(),
                Body = _authenticationRequestEncryptor.Encrypt(serverPublicKey, authRequestString),
                PublicKeyForReply = Convert.ToBase64String(_authenticationKeyProvider.PublicKey.Value)
            });

            var decryptedAuthBytes = _authenticationRequestEncryptor.Decrypt(_authenticationKeyProvider.PrivateKey, authResponseBytes);

            var authResponse = _objectSerializer.ToObject<EngineAuthenticationResponse>(decryptedAuthBytes);

            if (!authResponse.Success)
            {
                throw new ConfigurationErrorsException(authResponse.ErrorMessage);
            }

            #endregion
            
            #region Configuation

            var symmetricKeyPair = new SymmetricKeyPair
            {
                SymmetricKey = new SymmetricKey(Convert.FromBase64String(authResponse.SymmetricKey)),
                InitializationVector =
                    new InitializationVector(Convert.FromBase64String(authResponse.InitializationVector))
            };

            var requestString = _objectSerializer.ToBytes(request);

            var configurationBytes = _channel.ExecuteAndRespond(new SymmetricEnvelopeNeedingResponse
            {
                ResponseTypeName = typeof(EngineConfigurationResponse).AssemblyQualifiedName,
                KeyHash = symmetricKeyPair.SymmetricKey.GetHashString(),
                Body = _authenticatedCommunicationRequestEncryptor.Encrypt(symmetricKeyPair, requestString),
            });

            var decryptedConfigurationBytes = _authenticatedCommunicationRequestEncryptor.Decrypt(symmetricKeyPair, configurationBytes);

            return _objectSerializer.ToObject<EngineConfigurationResponse>(decryptedConfigurationBytes);
            #endregion
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}