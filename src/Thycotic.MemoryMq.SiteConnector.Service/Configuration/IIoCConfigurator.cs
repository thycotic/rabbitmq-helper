using System;
using System.Diagnostics.Contracts;
using System.Runtime.Remoting.Messaging;
using Autofac;
using Thycotic.DistributedEngine.Logic.Areas.POC;

namespace Thycotic.MemoryMq.SiteConnector.Service.Configuration
{
    /// <summary>
    /// Interface for an IoC Configirator
    /// </summary>
    [ContractClass(typeof(IoCConfiguratorContract))]
    public interface IIoCConfigurator
    {
        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="pipelineService">The PipelineService.</param>
        /// <returns></returns>
        IContainer BuildAll(SiteConnectorService pipelineService);
    }

    /// <summary>
    /// Contract For IIoCConfigurator
    /// </summary>
    [ContractClassFor(typeof(IIoCConfigurator))]
    public abstract class IoCConfiguratorContract : IIoCConfigurator
    {
        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="pipelineService">The PipelineService.</param>
        /// <returns></returns>
        public IContainer BuildAll(SiteConnectorService pipelineService)
        {
            Contract.Requires<ArgumentNullException>(pipelineService != null);
            return default(IContainer);
        }
    }
}