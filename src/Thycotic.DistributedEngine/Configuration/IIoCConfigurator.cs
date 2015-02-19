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
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        /// <returns></returns>
        IContainer Build(bool startConsuming);


    }
}