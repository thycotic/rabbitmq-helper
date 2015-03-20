using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class EngineConfigurationBus : IEngineConfigurationBus
    {
        private readonly IEngineToServerConnection _engineToServerConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        public EngineConfigurationBus(IEngineToServerConnection engineToServerConnection)
        {
            _engineToServerConnection = engineToServerConnection;
        }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            using (var channel = _engineToServerConnection.OpenChannel())
            {
                return channel.GetConfiguration(request);
            }
        }

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            using (var channel = _engineToServerConnection.OpenChannel())
            {
                return channel.SendHeartbeat(request);
            }
        }
    }
}