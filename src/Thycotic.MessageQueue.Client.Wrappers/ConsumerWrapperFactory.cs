using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Autofac;
using Autofac.Core;
using Thycotic.Messages.Common;
using Thycotic.Utility.Reflection;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Consumer wrapper factory
    /// </summary>
    public class ConsumerWrapperFactory : IStartable, IDisposable
    {
        private readonly IComponentContext _context;

        private readonly HashSet<IConsumerWrapperBase> _consumerWrappers = new HashSet<IConsumerWrapperBase>();
        private bool _disposed;

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
            StartActionConsumers(typeof(IBasicConsumer<>), typeof(BasicConsumerWrapper<,>));
            StartFunctionConsumers(typeof(IBlockingConsumer<,>), typeof(BlockingConsumerWrapper<,,>));
        }

        private void StartActionConsumers(Type baseConsumertype, Type wrapperType)
        {
            var consumerTypes =
                _context.ComponentRegistry.GetDistinctRegistrations()
                    .Where(r => r.Activator.LimitType.IsAssignableToGenericType(baseConsumertype));

            consumerTypes.ToList().ForEach(ct =>
            {
                var consumerType = ct.Activator.LimitType;
                var targetInterfaces = consumerType.GetInterfaces().Where(t => t.IsAssignableToGenericType(baseConsumertype));
                targetInterfaces.ToList().ForEach(ti =>
                {
                    var messageType = ti.GetGenericArguments()[0];

                    var consumerWrapperType = wrapperType.MakeGenericType(messageType, consumerType);

                    var consumerWrapper = (IConsumerWrapperBase)_context.Resolve(consumerWrapperType);

                    _consumerWrappers.Add(consumerWrapper);

                    consumerWrapper.StartConsuming();
                });
            });
        }

        private void StartFunctionConsumers(Type baseConsumertype, Type wrapperType)
        {
            var consumerTypes =
                _context.ComponentRegistry.GetDistinctRegistrations()
                    .Where(r => r.Activator.LimitType.IsAssignableToGenericType(baseConsumertype));

            consumerTypes.ToList().ForEach(ct =>
            {
                var consumerType = ct.Activator.LimitType;
                var targetInterfaces = consumerType.GetInterfaces().Where(t => t.IsAssignableToGenericType(baseConsumertype));
                targetInterfaces.ToList().ForEach(ti =>
                {
                    var messageType = ti.GetGenericArguments()[0];
                    var responseType = ti.GetGenericArguments()[1];

                    var consumerWrapperType = wrapperType.MakeGenericType(messageType, responseType, consumerType);

                    var consumerWrapper = (IConsumerWrapperBase)_context.Resolve(consumerWrapperType);

                    _consumerWrappers.Add(consumerWrapper);

                    consumerWrapper.StartConsuming();
                });
            });
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            var exceptions = new List<Exception>();

            _consumerWrappers.ToList().ForEach(cw =>
            {
                try
                {
                    cw.Dispose();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            });

            if (exceptions.Any())
            {
                throw new AggregateException("Could not dispose the factory", exceptions);
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class AutofacConsumerRegistrationExtensions
    {
        /// <summary>
        /// Gets the distinct registrations. Addresses issue where when interfaces are implemented by a type, the type will be returned more than once.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IEnumerable<IComponentRegistration> GetDistinctRegistrations(this IComponentRegistry registry)
        {
            return registry.Registrations.GroupBy(r => r.Activator.LimitType).Select(grp => grp.First());
        }
    }
}
