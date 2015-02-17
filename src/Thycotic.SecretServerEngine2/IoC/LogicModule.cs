using System;
using System.Reflection;
using Autofac;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.SecretServerEngine2.Logic.Areas.POC;
using Thycotic.Utility;
using Thycotic.Utility.Reflection;
using Module = Autofac.Module;

namespace Thycotic.SecretServerEngine2.IoC
{
    class LogicModule : Module
    {
        private readonly ILogWriter _log = Log.Get(typeof(LogicModule));

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing consumers...");

            LoadConsumers(builder, typeof (IBasicConsumer<>));
            LoadConsumers(builder, typeof (IBlockingConsumer<,>));
        }

        private void LoadConsumers(ContainerBuilder builder, Type type)
        {
            var logicAssembly = Assembly.GetAssembly(typeof (HelloWorldConsumer));

            _log.Debug(string.Format("Registering consumers of type {0}", type));

            builder.RegisterAssemblyTypes(logicAssembly)
                .Where(t => t.IsAssignableToGenericType(type))
                .AsSelf() //wrappers use explicit names through constructor types
                .AsImplementedInterfaces() //all other resolution
                .InstancePerDependency();
        }
    }
}
