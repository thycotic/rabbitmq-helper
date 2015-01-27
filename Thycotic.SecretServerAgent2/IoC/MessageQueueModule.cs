using System;
using System.Reflection;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages;
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

            var messageAssembly = Assembly.GetAssembly(typeof (IConsumable));
            builder.RegisterAssemblyTypes(messageAssembly).Where(t => typeof (IConsumable).IsAssignableFrom(t)).InstancePerDependency();

            var connectionString = _configurationProvider("RabbitMq.ConnectionString");
            _log.Info(string.Format("RabbitMq connection is {0}", connectionString));

            //builder.RegisterType<UnboundedChannelProvider>().As<IUnboundedChannelProvider>().SingleInstance();
            //builder.RegisterType<ConfigurationManagerWrapper>().As<IConfigurationManager>().SingleInstance();
            //builder.RegisterType<EventHandlerConfigProvider>().As<IEventHandlerConfigProvider>().SingleInstance();
            builder.RegisterType<JsonMessageSerializer>().As<IMessageSerializer>().SingleInstance();
            builder.Register(context => new RabbitMqConnection(connectionString))
                .As<IRabbitMqConnection>()
                .SingleInstance();
            builder.RegisterType<RabbitMqMessageBus>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<RabbitMqConsumersSetup>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterGeneric(typeof(AutofacRpcConsumer<,,>)).AsSelf().InstancePerDependency();
            //builder.RegisterGeneric(typeof(AutofacConsumer<,>)).AsSelf().InstancePerDependency();
            //builder.RegisterGeneric(typeof(AutofacBatchConsumer<,>)).AsSelf().InstancePerDependency();

        }
    }
}
