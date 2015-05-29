using Autofac;

namespace Thycotic.MemoryMq.SiteConnector.Service.Configuration
{
    /// <summary>
    /// Interface for an IoC Configirator
    /// </summary>
    public interface IIoCConfigurator
    {
        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="pipelineService">The PipelineService.</param>
        /// <returns></returns>
        IContainer BuildAll(PipelineService pipelineService);
        
        
    }
}