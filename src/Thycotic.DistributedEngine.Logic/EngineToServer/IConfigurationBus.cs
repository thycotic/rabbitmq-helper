using System;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Interface for a configuration provider from secret server
    /// </summary>
    public interface IConfigurationBus : IDisposable
    {
        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request);

    }
}
