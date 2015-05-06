using Autofac;
using Thycotic.DistributedEngine.Service.Update;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.IoC
{
    class UpdateModule : Module
    {
        private readonly ILogWriter _log = Log.Get(typeof(UpdateModule));

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing service updater...");

            builder.RegisterType<ServiceUpdaterWrapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
