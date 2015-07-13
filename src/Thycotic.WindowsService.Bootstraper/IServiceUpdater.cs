using System.Diagnostics.Contracts;

namespace Thycotic.WindowsService.Bootstraper
{
    /// <summary>
    /// Interface for a service updater
    /// </summary>
    [ContractClass(typeof(ServiceUpdaterContract))]
    public interface IServiceUpdater
    {
        /// <summary>
        /// Updates the service.
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Contract for IServerUpdater
    /// </summary>
    [ContractClassFor(typeof(IServiceUpdater))]
    public abstract class ServiceUpdaterContract : IServiceUpdater
    {
        /// <summary>
        /// Updates the service.
        /// </summary>
        public void Update()
        {
        }
    }
}