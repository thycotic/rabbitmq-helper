using System;
using System.Linq;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.MessageQueueClient.QueueClient.MemoryMq;
using Thycotic.MessageQueueClient.QueueClient.RabbitMq;

namespace Thycotic.SecretServerEngine2.IoC
{
    class MessageQueueModule : Module
    {
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(MessageQueueModule));

        public MessageQueueModule(Func<string, string> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing message queue dependencies...");

            builder.RegisterType<JsonMessageSerializer>().As<IMessageSerializer>().SingleInstance();

            var queueType = _configurationProvider(ConfigurationKeys.QueueType);
            
            if (queueType == SupportedMessageQueues.RabbitMq)
            {
                _log.Info("Using RabbitMq");

                var connectionString = _configurationProvider(ConfigurationKeys.RabbitMq.ConnectionString);
                _log.Info(string.Format("RabbitMq connection is {0}", connectionString));

                var userName = _configurationProvider(ConfigurationKeys.RabbitMq.UserName);
                _log.Info(string.Format("RabbitMq username is {0}", userName));

                var password = _configurationProvider(ConfigurationKeys.RabbitMq.Password);
                _log.Info(string.Format("RabbitMq password is {0}", string.Join("", Enumerable.Range(0, password.Length).Select(i => "*"))));

                var useSsl = Convert.ToBoolean(_configurationProvider(ConfigurationKeys.RabbitMq.UseSSL));
                if (useSsl)
                {
                    _log.Info("RabbitMq using encryption");
                }
                else
                {
                    _log.Warn("RabbitMq is not using encryption");
                }

                builder.Register(context => new RabbitMqConnection(connectionString, userName, password, useSsl))
                    .As<ICommonConnection>().InstancePerDependency();

            }
            else
            {
                _log.Info("Using MemoryMq");

                //initialize if necessary server 
                builder.RegisterModule(new MemoryMqServerModule(_configurationProvider));

                var connectionString = _configurationProvider(ConfigurationKeys.MemoryMq.ConnectionString);
                _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

                var useSsl = Convert.ToBoolean(_configurationProvider(ConfigurationKeys.MemoryMq.UseSSL));
                if (useSsl)
                {
                    _log.Info("MemoryMq using encryption");
                }
                else
                {
                    _log.Warn("MemoryMq is not using encryption");
                }


                builder.Register(context => new MemoryMqConnection(connectionString, useSsl))
                    .As<ICommonConnection>().InstancePerDependency();
            }

            builder.RegisterType<RequestBus>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
