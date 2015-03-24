using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Security;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.EngineToServer
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class ResponseBus : IResponseBus
    {
        private readonly IEngineToServerChannel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="engineToServerEncryptor">The message encryptor.</param>
        public ResponseBus(IEngineToServerConnection engineToServerConnection, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor)
        {
            _channel = engineToServerConnection.OpenChannel(objectSerializer, engineToServerEncryptor);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _channel.Dispose();
        }

        /// <summary>
        /// Pings the specified envelope.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Ping(EnginePingRequest request)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sends the secret heartbeat response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SendSecretHeartbeatResponse(SecretHeartbeatResponse response)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sends the remote password change response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SendRemotePasswordChangeResponse(RemotePasswordChangeResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}