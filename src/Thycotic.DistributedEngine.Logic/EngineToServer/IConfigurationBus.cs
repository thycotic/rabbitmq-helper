using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Interface for a configuration provider from secret server
    /// </summary>
    [ContractClass(typeof(ConfigurationBusContract))]
    public interface IConfigurationBus : IDisposable
    {
        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request);

    }

    /// <summary>
    /// Contract class for IConfigurationBus
    /// </summary>
    [ContractClassFor(typeof(IConfigurationBus))]
    public abstract class ConfigurationBusContract : IConfigurationBus
    {
        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            return default(EngineConfigurationResponse);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
