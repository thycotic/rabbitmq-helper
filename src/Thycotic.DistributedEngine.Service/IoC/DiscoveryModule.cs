using System;
using Autofac;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.Service.Heartbeat;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.IoC
{
    class DiscoveryModule : Module
    {
        private readonly ILogWriter _log = Log.Get(typeof(DiscoveryModule));

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            using (LogContext.Create("Engine Discovery"))
            {
                _log.Debug("Initializing Discovery...");
                builder.RegisterType<ScannerFactory>().AsImplementedInterfaces().SingleInstance();
            }
        }
    }
}
