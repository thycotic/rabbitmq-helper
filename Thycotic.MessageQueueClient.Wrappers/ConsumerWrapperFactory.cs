using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using RabbitMQ.Client;
using Thycotic.Utility;

namespace Thycotic.MessageQueueClient.Wrappers
{
    public class ConsumerWrapperFactory : IConsumerWrapperFactory, IStartable
    {
        private readonly IComponentContext _context;

        private readonly HashSet<IConsumerWrapperBase> _consumerWrappers = new HashSet<IConsumerWrapperBase>();

        public ConsumerWrapperFactory(IComponentContext context)
        {
            _context = context;
        }

        public void Start()
        {
            StartConsumers(typeof (IConsumer<>));
        }

        private void StartConsumers(Type type)
        {
            var wrapperType = typeof (SimpleConsumerWrapper<,>);

            var consumerTypes = _context.ComponentRegistry.Registrations.Where(r => r.Activator.LimitType.IsAssignableToGenericType(type));

            consumerTypes.ToList().ForEach(ct =>
            {
                var consumerType = ct.Activator.LimitType;
                var targetInterface = consumerType.GetInterfaces().Single(t => t.IsAssignableToGenericType(type));
                var messageType = targetInterface.GetGenericArguments()[0];

                var consumerWrapperType = wrapperType.MakeGenericType(messageType, consumerType);

                var consumerWrapper = (IConsumerWrapperBase)_context.Resolve(consumerWrapperType);

                _consumerWrappers.Add(consumerWrapper);

                consumerWrapper.StartConsuming();
                //consumer

            });
        }
    }
}
