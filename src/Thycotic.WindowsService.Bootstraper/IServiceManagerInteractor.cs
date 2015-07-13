using System.CodeDom;
using System.Diagnostics.Contracts;

namespace Thycotic.WindowsService.Bootstraper
{
    /// <summary>
    /// Interface for a service manager interactor
    /// </summary>
    [ContractClass(typeof(ServiceManagerInteractorContract))]
    public interface IServiceManagerInteractor
    {
        /// <summary>
        /// Starts the service.
        /// </summary>
        void StartService();

        /// <summary>
        /// Stops the service.
        /// </summary>
        void StopService();
    }

    /// <summary>
    /// Contract for IServiceManagerInteractor
    /// </summary>
    [ContractClassFor(typeof(IServiceManagerInteractor))]
    public abstract class ServiceManagerInteractorContract : IServiceManagerInteractor
    {
        /// <summary>
        /// Starts the service.
        /// </summary>
        public void StartService()
        {
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void StopService()
        {
        }
    }

}