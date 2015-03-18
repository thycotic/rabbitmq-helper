using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class EngineConfigurationBus : EngineToServerCommunicationWrapper, IEngineConfigurationBus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public EngineConfigurationBus(string connectionString, bool useSsl) : base(connectionString, useSsl) { }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            return Channel.GetConfiguration(request);
        }

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            return Channel.SendHeartbeat(request);
        }
    }
}