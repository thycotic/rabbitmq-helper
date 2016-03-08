using Autofac;
using Thycotic.ActiveDirectory;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.IoC
{
    class ActiveDirectoryModule : Module
    {
        private readonly ILogWriter _log = Log.Get(typeof(ActiveDirectoryModule));

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            using (LogContext.Create("EngineActiveDirectory"))
            {
                _log.Debug("Initializing Active Directory...");
                builder.RegisterType<ActiveDirectorySearcher>()
                    .AsSelf()
                    .AsImplementedInterfaces()
                    .InstancePerDependency();
            }
        }
    }
}