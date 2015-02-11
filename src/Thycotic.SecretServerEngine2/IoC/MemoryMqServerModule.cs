using System;
using System.IdentityModel.Selectors;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.SecretServerEngine2.MemoryMq;

namespace Thycotic.SecretServerEngine2.IoC
{
    class MemoryMqServerModule : Module
    {
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(MessageQueueModule));

        public MemoryMqServerModule(Func<string, string> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var startServerString = _configurationProvider(ConfigurationKeys.MemoryMq.StartServer);

            bool startServer;
            if (!Boolean.TryParse(startServerString, out startServer))
            {
                return;
            }

            if (!startServer)
            {
                return;
            }

            _log.Debug("Initializing Memory Mq server...");

            var connectionString = _configurationProvider(ConfigurationKeys.MemoryMq.ConnectionString);
            _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

            var thumbprint = _configurationProvider(ConfigurationKeys.MemoryMq.Thumbprint);
            _log.Info(string.Format("MemoryMq server thumbprint is {0}", thumbprint));

            builder.RegisterType<EngineClientVerifier>()
                .AsImplementedInterfaces()
                .As<UserNamePasswordValidator>()
                .InstancePerDependency();

            builder.Register(context => new MemoryMqServer(connectionString, thumbprint, context.Resolve<UserNamePasswordValidator>())).As<IStartable>().SingleInstance();
        }
    }
}
