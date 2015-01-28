using System;
using System.Reflection;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.MessageQueueClient.Wrappers;
using Thycotic.SecretServerAgent2.Logic.Areas.POC;
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

            var connectionString = _configurationProvider("RabbitMq.ConnectionString");
            _log.Info(string.Format("RabbitMq connection is {0}", connectionString));

            builder.RegisterType<JsonMessageSerializer>().As<IMessageSerializer>().SingleInstance();
            builder.Register(context => new RabbitMqConnection(connectionString))
                .As<IRabbitMqConnection>()
                .SingleInstance();
            builder.RegisterType<RabbitMqMessageBus>().AsImplementedInterfaces().SingleInstance();

            LoadConsumers(builder, typeof(IConsumer<>));
            LoadConsumers(builder, typeof(IConsumer<,>));
            LoadConsumers(builder, typeof(IRpcConsumer<,>));

            builder.RegisterType<ConsumerWrapperFactory>().SingleInstance();

        }

        private static void LoadConsumers(ContainerBuilder builder, Type type)
        {
            var logicAssembly = Assembly.GetAssembly(typeof (HelloWorldConsumer));

            builder.RegisterAssemblyTypes(logicAssembly).Where(t => t.IsAssignableToGenericType(type)).InstancePerDependency();
        }
    }
}
