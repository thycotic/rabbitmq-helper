using System;
using System.IdentityModel.Selectors;
using Autofac;
using Thycotic.Logging;
using Thycotic.SecretServerEngine.MemoryMq;

namespace Thycotic.SecretServerEngine.IoC
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

            var startServerString = _configurationProvider(MessageQueueClient.ConfigurationKeys.MemoryMq.Server.Start);

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

            var connectionString = _configurationProvider(MessageQueueClient.ConfigurationKeys.MemoryMq.ConnectionString);
            _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

            builder.RegisterType<EngineClientVerifier>()
                .AsImplementedInterfaces()
                .As<UserNamePasswordValidator>()
                .InstancePerDependency();


            var useSsl = Convert.ToBoolean(_configurationProvider(MessageQueueClient.ConfigurationKeys.MemoryMq.UseSsl));
            if (useSsl)
            {
                var thumbprint = _configurationProvider(MessageQueueClient.ConfigurationKeys.MemoryMq.Server.Thumbprint);
                _log.Info(string.Format("MemoryMq server thumbprint is {0}", thumbprint));


                builder.Register(context => new MemoryMqServer(connectionString, thumbprint, context.Resolve<UserNamePasswordValidator>()))
                    .As<IStartable>()
                    .SingleInstance();
            }
            else
            {

                builder.Register(context => new MemoryMqServer(connectionString, context.Resolve<UserNamePasswordValidator>()))
                    .As<IStartable>()
                    .SingleInstance();
            }
        }
    }
}
