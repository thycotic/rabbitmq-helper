using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Autofac;
using Thycotic.DistributedEngine.Service.Security;

namespace Thycotic.DistributedEngine.Service.Configuration
{
    /// <summary>
    /// Interface for an IoC configurator
    /// </summary>
    [ContractClass(typeof(IoCConfiguratorContract))]
    public interface IIoCConfigurator
    {
        /// <summary>
        /// The authentication key provider
        /// </summary>
        IAuthenticationKeyProvider AuthenticationKeyProvider { get; set; }

        /// <summary>
        /// The identity unique identifier provider
        /// </summary>
        IIdentityGuidProvider IdentityGuidProvider { get; set; }

        /// <summary>
        /// Gets or sets the last configuration consume.
        /// </summary>
        /// <value>
        /// The last configuration consume.
        /// </value>
        DateTime LastConfigurationConsumed { get; set; }

        /// <summary>
        /// Tries the assign configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        bool TryAssignConfiguration(Dictionary<string, string> configuration);

        /// <summary>
        /// Tries the get remote configuration.
        /// </summary>
        /// <param name="updateNeeded">if set to <c>true</c> [update needed].</param>
        /// <returns></returns>
        bool TryGetAndAssignConfiguration(out bool updateNeeded);

        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="engineService">The engineService.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start engineService].</param>
        /// <returns></returns>
        IContainer BuildAll(EngineService engineService, bool startConsuming);


        /// <summary>
        /// Set Configuration to null
        /// </summary>
        /// <returns></returns>
        void ClearConfiguration();
    }

    /// <summary>
    /// Contract for IIoCConfigurator
    /// </summary>
    [ContractClassFor(typeof(IIoCConfigurator))]
    public abstract class IoCConfiguratorContract : IIoCConfigurator
    {
        /// <summary>
        /// The authentication key provider
        /// </summary>
        public IAuthenticationKeyProvider AuthenticationKeyProvider { get; set; }

        /// <summary>
        /// The identity unique identifier provider
        /// </summary>
        public IIdentityGuidProvider IdentityGuidProvider { get; set; }

        /// <summary>
        /// Gets or sets the last configuration consume.
        /// </summary>
        /// <value>
        /// The last configuration consume.
        /// </value>
        public DateTime LastConfigurationConsumed { get; set; }

        /// <summary>
        /// Tries the assign configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public bool TryAssignConfiguration(Dictionary<string, string> configuration)
        {
            return default(bool);
        }

        /// <summary>
        /// Tries the get remote configuration.
        /// </summary>
        /// <param name="updateNeeded">if set to <c>true</c> [update needed].</param>
        /// <returns></returns>
        public bool TryGetAndAssignConfiguration(out bool updateNeeded)
        {
            Contract.ValueAtReturn(out updateNeeded);

            return default(bool);
        }

        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="engineService">The engineService.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start engineService].</param>
        /// <returns></returns>
        public IContainer BuildAll(EngineService engineService, bool startConsuming)
        {
            Contract.Requires<ArgumentNullException>(engineService != null);

            return default(IContainer);
        }


        /// <summary>
        /// Set Configuration to null
        /// </summary>
        /// <returns></returns>
        public void ClearConfiguration()
        {
        }
    }
}