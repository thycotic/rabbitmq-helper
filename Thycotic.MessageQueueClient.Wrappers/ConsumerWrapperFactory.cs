using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Thycotic.Messages.Common;
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
            StartActionConsumers(typeof (IConsumer<>), typeof(SimpleConsumerWrapper<,>));
            StartFunctionConsumers(typeof(IRpcConsumer<,>), typeof(RpcConsumerWrapper<,,>));
        }

        private void StartActionConsumers(Type baseConsumertype, Type wrapperType)
        {
            var consumerTypes = _context.ComponentRegistry.Registrations.Where(r => r.Activator.LimitType.IsAssignableToGenericType(baseConsumertype));

            consumerTypes.ToList().ForEach(ct =>
            {
                var consumerType = ct.Activator.LimitType;
                var targetInterface = consumerType.GetInterfaces().Single(t => t.IsAssignableToGenericType(baseConsumertype));
                var messageType = targetInterface.GetGenericArguments()[0];

                var consumerWrapperType = wrapperType.MakeGenericType(messageType, consumerType);

                var consumerWrapper = (IConsumerWrapperBase)_context.Resolve(consumerWrapperType);

                _consumerWrappers.Add(consumerWrapper);

                consumerWrapper.StartConsuming();
                //consumer

            });
        }

        private void StartFunctionConsumers(Type baseConsumertype, Type wrapperType)
        {
            var consumerTypes = _context.ComponentRegistry.Registrations.Where(r => r.Activator.LimitType.IsAssignableToGenericType(baseConsumertype));

            consumerTypes.ToList().ForEach(ct =>
            {
                var consumerType = ct.Activator.LimitType;
                var targetInterface = consumerType.GetInterfaces().Single(t => t.IsAssignableToGenericType(baseConsumertype));
                var messageType = targetInterface.GetGenericArguments()[0];
                var responseType = targetInterface.GetGenericArguments()[1];

                var consumerWrapperType = wrapperType.MakeGenericType(messageType, responseType, consumerType);

                var consumerWrapper = (IConsumerWrapperBase)_context.Resolve(consumerWrapperType);

                _consumerWrappers.Add(consumerWrapper);

                consumerWrapper.StartConsuming();
            });
        }
    }
}
