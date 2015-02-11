using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Thycotic.Messages.Common;
using Thycotic.Utility;

namespace Thycotic.MessageQueueClient.Wrappers
{
    /// <summary>
    /// Consumer wrapper factory
    /// </summary>
    public class ConsumerWrapperFactory : IStartable
    {
        private readonly IComponentContext _context;

        private readonly HashSet<IConsumerWrapperBase> _consumerWrappers = new HashSet<IConsumerWrapperBase>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerWrapperFactory"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ConsumerWrapperFactory(IComponentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            StartActionConsumers(typeof (IBasicConsumer<>), typeof(BasicConsumerWrapper<,>));
            StartFunctionConsumers(typeof(IBlockingConsumer<,>), typeof(BlockingConsumerWrapper<,,>));
        }

        private void StartActionConsumers(Type baseConsumertype, Type wrapperType)
        {
            var consumerTypes = _context.ComponentRegistry.Registrations.Where(r => r.Activator.LimitType.IsAssignableToGenericType(baseConsumertype));

            consumerTypes.ToList().ForEach(ct =>
            {
                var consumerType = ct.Activator.LimitType;
                var targetInterfaces = consumerType.GetInterfaces().Where(t => t.IsAssignableToGenericType(baseConsumertype));
                targetInterfaces.ToList().ForEach(ti =>
                {
                    var messageType = ti.GetGenericArguments()[0];

                    var consumerWrapperType = wrapperType.MakeGenericType(messageType, consumerType);

                    var consumerWrapper = (IConsumerWrapperBase) _context.Resolve(consumerWrapperType);

                    _consumerWrappers.Add(consumerWrapper);

                    consumerWrapper.StartConsuming();
                });
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
