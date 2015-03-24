using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.EngineToServer
{
    /// <summary>
    /// Engine to server connection channel
    /// </summary>
    public class EngineToServerChannel : IEngineToServerChannel
    {
        private PublicKey _serverPublicKey;
        private readonly IEngineToServerCommunicationWcfService _connection;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IEngineToServerEncryptor _engineToServerEncryptor;


        /// <summary>
        /// Initializes a new instance of the <see cref="EngineToServerChannel" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="engineToServerEncryptor">The engine to server encryptor.</param>
        public EngineToServerChannel(IEngineToServerCommunicationWcfService connection, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor)
        {
            _connection = connection;
            _objectSerializer = objectSerializer;
            _engineToServerEncryptor = engineToServerEncryptor;
        }

        //private EncryptedEngineRequest WrapRequest(object request)
        //{
        //    var bytes = _objectSerializer.ToBytes(request);
        //    var body = _engineToServerEncryptor.EncryptWithServerPublicKey(_serverPublicKey, bytes);

        //    return new EncryptedEngineRequest
        //    {
        //        Body = body
        //    };
        //}

        //private T UnWrapResponse<T>(EncryptedEngineResponse response)
        //{
        //    var body = _engineToServerEncryptor.DecryptWithPrivateKey(response.Body);

        //    return _objectSerializer.ToObject<T>(body);
        //}

        /// <summary>
        /// Pre-authentication request to get a public key for server so that subsequent request are encrypted with that key
        /// </summary>
        /// <returns></returns>
        public void PreAuthenticate()
        {
            var bytes = _connection.PreAuthenticate();

            var response = _objectSerializer.ToObject<EnginePreAuthenticationResponse>(bytes);

            _serverPublicKey = new PublicKey(Convert.FromBase64String(response.ServerPublicKey));
        }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            //var bytes = _connection.GetConfiguration(envelope);

            return null;
        }

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            //var bytes = _connection.SendHeartbeat(envelope);
            return null;
        }

        /// <summary>
        /// Pings the specified envelope.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Ping(EnginePingRequest request)
        {
         
        }

        /// <summary>
        /// Sends the secret heartbeat response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SendSecretHeartbeatResponse(SecretHeartbeatResponse response)
        {
         
        }

        /// <summary>
        /// Sends the remote password change response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SendRemotePasswordChangeResponse(RemotePasswordChangeResponse response)
        {
         
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //nothing to dispose
        }
    }
}