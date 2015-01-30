using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Thycotic.MessageQueueClient.Wrappers.RabbitMq;
using Thycotic.Messages.Common;
using Thycotic.Utility;

namespace Thycotic.MessageQueueClient.Wrappers
{
    /// <summary>
    /// Consumer wrapper factory
    /// </summary>
    public class ConsumerWrapperFactory : IStartable
    {
        /// <summary>
        /// The basic consumer type parameter name
        /// </summary>
        public const string BasicConsumerTypeParameterName = "basicConsumerType";

        /// <summary>
        /// The blocking consumer type parameter name
        /// </summary>
        public const string BlockingConsumerTypeParameterName = "blockingConsumerType";

        private readonly IComponentContext _context;
        private readonly Type _basicConsumerType;
        private readonly Type _blockingConsumerType;

        private readonly HashSet<IConsumerWrapperBase> _consumerWrappers = new HashSet<IConsumerWrapperBase>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerWrapperFactory"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="basicConsumerType"></param>
        /// <param name="blockingConsumerType"></param>
        public ConsumerWrapperFactory(IComponentContext context, Type basicConsumerType, Type blockingConsumerType)
        {
            _context = context;
            _basicConsumerType = basicConsumerType;
            _blockingConsumerType = blockingConsumerType;
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            StartBasicConsumers(typeof (IBasicConsumer<>), _basicConsumerType);
            StartBlockingConsumers(typeof(IBlockingConsumer<,>), _blockingConsumerType);
        }

        private void StartBasicConsumers(Type baseConsumertype, Type wrapperType)
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

        private void StartBlockingConsumers(Type baseConsumertype, Type wrapperType)
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
