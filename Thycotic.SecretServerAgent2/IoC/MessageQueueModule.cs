using System;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.MemoryMq;
using Thycotic.MessageQueueClient.RabbitMq;
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

            builder.RegisterType<JsonMessageSerializer>().As<IMessageSerializer>().InstancePerDependency();

            var queueType = _configurationProvider("Queue.Type");

            if (queueType == SupportedMessageQueues.RabbitMq)
            {
                _log.Info("Using RabbitMq");
                var connectionString = _configurationProvider("RabbitMq.ConnectionString");
                _log.Info(string.Format("RabbitMq connection is {0}", connectionString));

                builder.Register(context => new RabbitMqConnection(connectionString))
                    .As<IRabbitMqConnection>()
                    .InstancePerDependency();
                builder.RegisterType<RabbitMqRequestBus>().AsImplementedInterfaces().SingleInstance();
            }
            else
            {
                _log.Info("Using MemoryMq");

                builder.RegisterType<MemoryMqConnection>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<MemoryMqRequestBus>().AsImplementedInterfaces().SingleInstance();
            }
        }
    }
}
