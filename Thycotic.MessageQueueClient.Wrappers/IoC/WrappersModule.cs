using System;
using Autofac;
using Autofac.Core;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.Wrappers.MemoryMq;
using Thycotic.MessageQueueClient.Wrappers.RabbitMq;
using Thycotic.Messages.Common;
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

            var queueType = _configurationProvider("Queue.Type");

            Type typeBasicConsumer;
            Type typeBlockingConsumer;

            if (queueType == SupportedMessageQueues.RabbitMq)
            {
                _log.Info("Using RabbitMq wrappers");

                typeBasicConsumer = typeof(BasicRabbitMqConsumerWrapper<,>);
                typeBlockingConsumer = typeof(BlockingRabbitMqConsumerWrapper<,,>);
            }
            else //MemoryMq or something unsupported
            {
                _log.Info("Using MemoryMq wrappers");

                typeBasicConsumer = typeof(BasicMemoryMqConsumerWrapper<,>);
                typeBlockingConsumer = typeof(BlockingMemoryMqConsumerWrapper<,,>);
            }

            builder.RegisterGeneric(typeBasicConsumer).InstancePerDependency();
            builder.RegisterGeneric(typeBlockingConsumer).InstancePerDependency();

            builder.RegisterType<ConsumerWrapperFactory>().WithParameters(new[]
                {
                    new ResolvedParameter(
                        (pi, ctx) => pi.Name == ConsumerWrapperFactory.BasicConsumerTypeParameterName,
                        (pi, ctx) => typeBasicConsumer),
                    new ResolvedParameter(
                        (pi, ctx) => pi.Name == ConsumerWrapperFactory.BlockingConsumerTypeParameterName,
                        (pi, ctx) => typeBlockingConsumer)

                }).As<IStartable>().SingleInstance();
        }
    }
}
