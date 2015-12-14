using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Thycotic.Logging;
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

        private readonly ILogWriter _log = Log.Get(typeof (ConsumerWrapperFactory));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerWrapperFactory"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ConsumerWrapperFactory(IComponentContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);

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

                    var prioritySchedulerProvider = _context.Resolve<IPrioritySchedulerProvider>();

                    var consumerWrapper = (IConsumerWrapperBase)_context.Resolve(consumerWrapperType);
                    var customPriorityAttribute = (ConsumerPriority)consumerType.GetCustomAttribute(typeof(ConsumerPriority));
                    if (customPriorityAttribute != null)
                    {
                        switch (customPriorityAttribute.Priority)
                        {
                            case Priority.BelowNormal:
                                consumerWrapper.SetPriority(prioritySchedulerProvider.BelowNormal);
                                break;
                            case Priority.Normal:
                                consumerWrapper.SetPriority(prioritySchedulerProvider.Normal);
                                break;
                            case Priority.AboveNormal:
                                consumerWrapper.SetPriority(prioritySchedulerProvider.AboveNormal);
                                break;
                            case Priority.Highest:
                                consumerWrapper.SetPriority(prioritySchedulerProvider.Highest);
                                break;
                        }
                    }
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

            _consumerWrappers.AsParallel().ForAll(cw =>
            {
                try
                {
                    _log.Debug(string.Format("Disposing {0}", cw.GetType().FullName));

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
            Contract.Requires(registry != null);
            Contract.Ensures(Contract.Result<IEnumerable<IComponentRegistration>>() != null); 

            return registry.Registrations.GroupBy(r => r.Activator.LimitType).Select(grp => grp.First());
        }
    }
}
