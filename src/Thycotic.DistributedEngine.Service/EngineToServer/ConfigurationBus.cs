using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Logic.Update;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;
using PublicKey = Thycotic.Encryption.PublicKey;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class ConfigurationBus : BasicBus, IConfigurationBus
    {
        private readonly IObjectSerializer _objectSerializer;
        private readonly IAuthenticationKeyProvider _authenticationKeyProvider;
        private readonly IAuthenticationRequestEncryptor _authenticationRequestEncryptor;
        private readonly IAuthenticatedCommunicationRequestEncryptor _authenticatedCommunicationRequestEncryptor;
        private EngineToServerCommunicationCallback _callback = new EngineToServerCommunicationCallback();
        

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnectionManager">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticationKeyProvider">The authentication key provider.</param>
        /// <param name="authenticationRequestEncryptor">The authentication request encryptor.</param>
        /// <param name="authenticatedCommunicationRequestEncryptor">The authenticated communication request encryptor.</param>
        public ConfigurationBus(IEngineToServerConnectionManager engineToServerConnectionManager,
            IObjectSerializer objectSerializer,
            IAuthenticationKeyProvider authenticationKeyProvider,
            IAuthenticationRequestEncryptor authenticationRequestEncryptor,
            IAuthenticatedCommunicationRequestEncryptor authenticatedCommunicationRequestEncryptor) 
            : base(engineToServerConnectionManager)
        {
            Contract.Requires<ArgumentNullException>(engineToServerConnectionManager != null);
            _objectSerializer = objectSerializer;
            _authenticationKeyProvider = authenticationKeyProvider;
            _authenticationRequestEncryptor = authenticationRequestEncryptor;
            _authenticatedCommunicationRequestEncryptor = authenticatedCommunicationRequestEncryptor;
        }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            return WrapInteraction(channel =>
            {
                #region Pre-auth

                var preAuthResponseBytes = channel.PreAuthenticate();

                var preAuthentication = _objectSerializer.ToObject<EnginePreAuthenticationResponse>(preAuthResponseBytes);

                if (!preAuthentication.Success)
                {
                    throw new ConfigurationErrorsException(preAuthentication.ErrorMessage);
                }
                
                #endregion

                #region Auth

                var serverPublicKey = new PublicKey(Convert.FromBase64String(preAuthentication.PublicKeyForReply));

                var authRequestString = _objectSerializer.ToBytes(request);

                var authResponseBytes = channel.Authenticate(new AsymmetricEnvelope
                {
                    KeyHash = serverPublicKey.GetHashString(),
                    Body = _authenticationRequestEncryptor.Encrypt(serverPublicKey, authRequestString),
                    PublicKeyForReply = Convert.ToBase64String(_authenticationKeyProvider.PublicKey.Value)
                });

                var decryptedAuthBytes = _authenticationRequestEncryptor.Decrypt(_authenticationKeyProvider.PrivateKey,
                    authResponseBytes);

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

                var configurationBytes = channel.ExecuteAndRespond(new SymmetricEnvelopeNeedingResponse
                {
                    ResponseTypeName = typeof (EngineConfigurationResponse).FullName,
                    KeyHash = symmetricKeyPair.SymmetricKey.GetHashString(),
                    Body = _authenticatedCommunicationRequestEncryptor.Encrypt(symmetricKeyPair, requestString),
                });

                var decryptedConfigurationBytes = _authenticatedCommunicationRequestEncryptor.Decrypt(symmetricKeyPair,
                    configurationBytes);

                return _objectSerializer.ToObject<EngineConfigurationResponse>(decryptedConfigurationBytes);

                #endregion
            }, _callback);
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose, everything is already disposed.
        }
    }
}