using System;
using Autofac;
using Thycotic.Logging;
using Module = Autofac.Module;

namespace Thycotic.MessageQueueClient.Wrappers.IoC
{
    /// <summary>
    /// Module to register wrappers and their factory
    /// </summary>
    public class WrappersModule : Module
    {
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(WrappersModule));

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappersModule"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        public WrappersModule(Func<string, string> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        /// <summary>
        /// Loads wrappers.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing consumer wrappers...");

            var queueType = _configurationProvider(ConfigurationKeys.QueueType);

            if (queueType == SupportedMessageQueues.RabbitMq)
            {
                _log.Info("Using RabbitMq wrappers");
                builder.RegisterGeneric(typeof (BasicConsumerWrapper<,>)).InstancePerDependency();
                builder.RegisterGeneric(typeof (BlockingConsumerWrapper<,,>)).InstancePerDependency();

            }
            else
            {
                _log.Info("Using MemoryMq wrappers");

                //builder.RegisterGeneric(typeof(BasicMemoryMqConsumerWrapper<,>)).InstancePerDependency();
                //builder.RegisterGeneric(typeof(BlockingMemoryConsumerWrapper<,,>)).InstancePerDependency();
            }

            builder.RegisterType<ConsumerWrapperFactory>().As<IStartable>().SingleInstance();
        }
    }
}
