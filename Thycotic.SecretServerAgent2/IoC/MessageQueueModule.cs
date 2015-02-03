using System;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.MessageQueueClient.QueueClient.MemoryMq;
using Thycotic.MessageQueueClient.QueueClient.RabbitMq;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.SecretServerAgent2.MemoryMq;
using Module = Autofac.Module;

namespace Thycotic.SecretServerAgent2.IoC
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

                builder.Register(context => new RabbitMqConnection(connectionString))
                    .As<ICommonConnection>().InstancePerDependency();

            }
            else
            {
                _log.Info("Using MemoryMq");

                var connectionString = _configurationProvider(ConfigurationKeys.MemoryMq.ConnectionString);
                _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

                var thumbprint = _configurationProvider(ConfigurationKeys.MemoryMq.Thumbprint);
                _log.Info(string.Format("MemoryMq server thumbprint is {0}", thumbprint));

                builder.Register(context => new MemoryMqServer(connectionString, thumbprint)).As<IStartable>().SingleInstance();

                builder.RegisterType<JsonMessageSerializer>().As<IMessageSerializer>().SingleInstance();
                builder.Register(context => new MemoryMqConnection(connectionString))
                    .As<ICommonConnection>().InstancePerDependency();
            }

            builder.RegisterType<RequestBus>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
