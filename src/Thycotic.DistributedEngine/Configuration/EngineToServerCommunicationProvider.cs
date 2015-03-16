using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Response;
using Thycotic.Wcf;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class EngineToServerCommunicationProvider : IEngineToServerCommunicationProvider
    {
        private readonly IEngineToServerCommunicationWcfService _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineToServerCommunicationProvider"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public EngineToServerCommunicationProvider(string uri, bool useSsl)
        {
            _channel = NetTcpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(uri, useSsl);
        }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            return _channel.GetConfiguration(request);
        }

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            return _channel.SendHeartbeat(request);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}