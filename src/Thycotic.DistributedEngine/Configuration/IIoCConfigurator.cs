using Autofac;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// Interface for an IoC Configirator
    /// </summary>
    public interface IIoCConfigurator
    {
        /// <summary>
        /// Tries the get remote configuration.
        /// </summary>
        /// <returns></returns>
        bool TryGetRemoteConfiguration();

        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="engineService">The engineService.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start engineService].</param>
        /// <returns></returns>
        IContainer Build(EngineService engineService, bool startConsuming);


    }
}