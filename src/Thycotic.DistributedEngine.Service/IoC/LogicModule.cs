using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.Messages.Common;
using Thycotic.Utility.Reflection;
using Module = Autofac.Module;

namespace Thycotic.DistributedEngine.Service.IoC
{
    class LogicModule : Module
    {
        private readonly Func<string, string> _configurationProvider;
        private readonly ILogWriter _log = Log.Get(typeof(LogicModule));

        public LogicModule(Func<string, string> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);


            builder.Register(context =>
            {
                var syncContext = SynchronizationContext.Current ?? new SynchronizationContext();

                var maximumThreadMultiplierString = _configurationProvider("MaximumThreadMultiplier");

                var maximumThreadMultiplier = !string.IsNullOrWhiteSpace(maximumThreadMultiplierString) ? Convert.ToInt32(maximumThreadMultiplierString) : 10;

                _log.Info(string.Format("Maximum thread multiplier is {0}", maximumThreadMultiplier));

                return new PrioritySchedulerProvider(syncContext, maximumThreadMultiplier);

            }).As<IPrioritySchedulerProvider>().SingleInstance();

            _log.Debug("Initializing consumers...");

            LoadConsumers(builder, typeof(IBasicConsumer<>));
            LoadConsumers(builder, typeof(IBlockingConsumer<,>));
        }

        private void LoadConsumers(ContainerBuilder builder, Type type)
        {
            var logicAssembly = Assembly.GetAssembly(typeof(HelloWorldConsumer));

            _log.Debug(string.Format("Registering consumers of type {0}", type));

            var eligibleConsumerTypes = logicAssembly.GetTypes()
                .Where(t => t.IsAssignableToGenericType(type))
#if !DEBUG
                .Where(t => !typeof(IRegisterForPocOnly).IsAssignableFrom(t)) //disabled POC functionality
#endif
                .ToList();

            eligibleConsumerTypes.ForEach(t => _log.Debug(string.Format("Registering consumer type {0}", t)));

            builder.RegisterAssemblyTypes(logicAssembly)
                .Where(t => t.IsAssignableToGenericType(type))
#if !DEBUG
                .Where(t => !typeof(IRegisterForPocOnly).IsAssignableFrom(t)) //disabled POC functionality
#endif
                .AsSelf() //wrappers use explicit names through constructor types
                .AsImplementedInterfaces() //all other resolution
                .InstancePerDependency();
        }
    }
}
