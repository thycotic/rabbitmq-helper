using System;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Logic
{
    /// <summary>
    /// Interface for a configuration provider from secret server
    /// </summary>
    public interface IEngineConfigurationBus: IDisposable
    {
        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request);

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request);


    }
}
