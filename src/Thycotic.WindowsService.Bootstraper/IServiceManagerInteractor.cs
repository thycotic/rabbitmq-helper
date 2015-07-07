using System.CodeDom;
using System.Diagnostics.Contracts;

namespace Thycotic.WindowsService.Bootstraper
{
    [ContractClass(typeof(ServiceManagerInteractorContract))]
    public interface IServiceManagerInteractor
    {
        void StartService();
        void StopService();
    }

    [ContractClassFor(typeof(IServiceManagerInteractor))]
    public abstract class ServiceManagerInteractorContract : IServiceManagerInteractor
    {
        public void StartService()
        {
        }

        public void StopService()
        {
        }
    }

}