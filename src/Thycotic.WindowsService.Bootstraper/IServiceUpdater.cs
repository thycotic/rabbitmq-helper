using System.Diagnostics.Contracts;

namespace Thycotic.WindowsService.Bootstraper
{
    [ContractClass(typeof(ServiceUpdaterContract))]
    public interface IServiceUpdater
    {
        void Update();
    }

    [ContractClassFor(typeof(IServiceUpdater))]
    public abstract class ServiceUpdaterContract : IServiceUpdater
    {
        public void Update()
        {
        }
    }
}