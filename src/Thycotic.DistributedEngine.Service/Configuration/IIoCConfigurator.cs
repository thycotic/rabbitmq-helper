using System;
using System.Collections.Generic;
using Autofac;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// Interface for an IoC Configirator
    /// </summary>
    public interface IIoCConfigurator
    {
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
        /// <returns></returns>
        bool TryGetAndAssignConfiguration();

        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="engineService">The engineService.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start engineService].</param>
        /// <returns></returns>
        IContainer BuildAll(EngineService engineService, bool startConsuming);
        
        
    }
}