using System;
using Autofac;
using Thycotic.DistributedEngine.Heartbeat;
using Thycotic.Logging;
using Module = Autofac.Module;

namespace Thycotic.DistributedEngine.IoC
{
    class HeartbeatModule : Module
    {
        private readonly EngineService _engineService;
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(HeartbeatModule));

        public HeartbeatModule(EngineService engineService, Func<string, string> configurationProvider)
        {
            _engineService = engineService;
            _configurationProvider = configurationProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing heartbeat...");

            var heartbeatIntervalSeconds =
                Convert.ToInt32(_configurationProvider(MessageQueue.Client.ConfigurationKeys.HeartbeatIntervalSeconds));

            builder.Register(context => new HeartbeatRunner(_engineService, heartbeatIntervalSeconds)).As<IStartable>().SingleInstance();
        }
    }
}
