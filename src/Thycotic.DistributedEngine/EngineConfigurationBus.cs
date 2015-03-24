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
        private readonly IAuthenticationRequestEncryptor _authenticationRequestEncryptor;
        private readonly IAuthenticatedCommunicationKeyProvider _authenticatedCommunicationKeyProvider;
        private readonly IAuthenticatedCommunicationRequestEncryptor _authenticatedCommunicationRequestEncryptor;
        private readonly IEngineToServerCommunicationWcfService _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticationRequestEncryptor">The authentication request encryptor.</param>
        /// <param name="authenticatedCommunicationKeyProvider">The authenticated communication key provider.</param>
        /// <param name="authenticatedCommunicationRequestEncryptor">The authenticated communication request encryptor.</param>
        public EngineConfigurationBus(IEngineToServerConnection engineToServerConnection, 
            IObjectSerializer objectSerializer, 
            IAuthenticationRequestEncryptor authenticationRequestEncryptor, 
            IAuthenticatedCommunicationKeyProvider authenticatedCommunicationKeyProvider, 
            IAuthenticatedCommunicationRequestEncryptor authenticatedCommunicationRequestEncryptor)
        {
        
            _objectSerializer = objectSerializer;
            _authenticationRequestEncryptor = authenticationRequestEncryptor;
            _authenticatedCommunicationKeyProvider = authenticatedCommunicationKeyProvider;
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
            var preAuthenticationBytes = _channel.PreAuthenticate();

            var preAuthentication = _objectSerializer.ToObject<EnginePreAuthenticationResponse>(preAuthenticationBytes);

            var serverPublicKey = new PublicKey(Convert.FromBase64String(preAuthentication.ServerPublicKey));

            var requestString = _objectSerializer.ToBytes(request);

            var configurationBytes = _channel.GetConfiguration(new AsymmetricEnvelope
            {
                KeyHash = serverPublicKey.GetHashString(),
                Body = _authenticationRequestEncryptor.Encrypt(serverPublicKey, requestString)
            });

            return _objectSerializer.ToObject<EngineConfigurationResponse>(configurationBytes);
        }

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            var requestString = _objectSerializer.ToBytes(request);

            var heartbeatBytes = _channel.SendHeartbeat(new SymmetricEnvelope
            {
                KeyHash = _authenticatedCommunicationKeyProvider.SymmetricKey.GetHashString(),
                Body = _authenticatedCommunicationRequestEncryptor.Encrypt((SymmetricKeyPair)_authenticatedCommunicationKeyProvider, requestString)
            });

            return _objectSerializer.ToObject<EngineHeartbeatResponse>(heartbeatBytes);
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