using System;
using Thycotic.DistributedEngine.EngineToServer;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class EngineConfigurationBus : IEngineConfigurationBus
    {
        private readonly IObjectSerializer _objectSerializer;
        private readonly IAuthenticationKeyProvider _authenticationKeyProvider;
        private readonly IAuthenticationRequestEncryptor _authenticationRequestEncryptor;
        private readonly IEngineToServerCommunicationWcfService _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticationKeyProvider">The authentication key provider.</param>
        /// <param name="authenticationRequestEncryptor">The authentication request encryptor.</param>
        public EngineConfigurationBus(IEngineToServerConnection engineToServerConnection, 
            IObjectSerializer objectSerializer, 
            IAuthenticationKeyProvider authenticationKeyProvider,
            IAuthenticationRequestEncryptor authenticationRequestEncryptor)
        {
        
            _objectSerializer = objectSerializer;
            _authenticationKeyProvider = authenticationKeyProvider;
            _authenticationRequestEncryptor = authenticationRequestEncryptor;
            _channel = engineToServerConnection.OpenChannel();
        }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            var preAuthenticationBytes = _channel.PreAuthenticate();

            var preAuthentication = _objectSerializer.ToObject<EnginePreAuthenticationResponse>(preAuthenticationBytes);

            var serverPublicKey = new PublicKey(Convert.FromBase64String(preAuthentication.PublicKeyForReply));

            var requestString = _objectSerializer.ToBytes(request);

            var configurationBytes = _channel.GetConfiguration(new AsymmetricEnvelope
            {
                KeyHash = serverPublicKey.GetHashString(),
                Body = _authenticationRequestEncryptor.Encrypt(serverPublicKey, requestString),
                PublicKeyForReply = Convert.ToBase64String(_authenticationKeyProvider.PublicKey.Value)
            });

            var decryptedConfigurationBytes = _authenticationRequestEncryptor.Decrypt(_authenticationKeyProvider.PrivateKey, configurationBytes);

            return _objectSerializer.ToObject<EngineConfigurationResponse>(decryptedConfigurationBytes);
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