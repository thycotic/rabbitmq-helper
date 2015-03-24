using System;
using Autofac;
using Thycotic.DistributedEngine.Heartbeat;
using Thycotic.Logging;
using Module = Autofac.Module;

namespace Thycotic.DistributedEngine.IoC
{
    class HeartbeatModule : Module
    {
        private readonly Func<string, string> _configurationProvider;
        private readonly EngineService _engineService;

        private readonly ILogWriter _log = Log.Get(typeof(HeartbeatModule));


        public HeartbeatModule(Func<string, string> configurationProvider, EngineService engineService)
        {
            _configurationProvider = configurationProvider;
            _engineService = engineService;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            using (LogContext.Create("Engine Heartbeat"))
            {
                
                _log.Debug("Initializing heartbeat...");

                var heartbeatIntervalSeconds =
                    Convert.ToInt32(
                        _configurationProvider(MessageQueue.Client.ConfigurationKeys.Engine.HeartbeatIntervalSeconds));

                builder.Register(context => _engineService).SingleInstance();

                builder.Register(content => new HeartbeatConfigurationProvider
                {
                    HeartbeatIntervalSeconds = heartbeatIntervalSeconds
                }).As<IHeartbeatConfigurationProvider>();
                builder.RegisterType<HeartbeatRunner>().As<IStartable>().SingleInstance();
            }
        }
    }
}
