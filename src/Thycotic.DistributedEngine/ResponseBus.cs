using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;
using Thycotic.Wcf;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class ResponseBus : IResponseBus
    {
        private readonly IEngineToServerCommunicationWcfService _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus"/> class.
        /// </summary>
        /// <param name="connectionString">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public ResponseBus(string connectionString, bool useSsl)
        {
            var uri = new Uri(connectionString);

            switch (uri.Scheme)
            {
                case "net.tcp":
                    _channel = NetTcpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(connectionString, useSsl);
                    break;
                case "http":
                case "https":
                    _channel = HttpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(connectionString, useSsl);
                    break;
            }
        }


        /// <summary>
        /// Not supported on the response bus
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported on the response bus
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// (Ping) Pong back to server
        /// </summary>
        public void Pong()
        {
            _channel.Pong();
        }

        /// <summary>
        /// Records the secret heartbeat response.
        /// </summary>
        /// <param name="response"></param>
        public void RecordSecretHeartbeatResponse(SecretHeartbeatResponse response)
        {
            _channel.RecordSecretHeartbeatResponse(response);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}