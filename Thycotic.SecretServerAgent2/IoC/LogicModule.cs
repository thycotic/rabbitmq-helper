using System;
using System.Reflection;
using Autofac;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.SecretServerAgent2.Logic.Areas.POC;
using Thycotic.Utility;
using Module = Autofac.Module;

namespace Thycotic.SecretServerAgent2.IoC
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

            builder.RegisterAssemblyTypes(logicAssembly).Where(t => t.IsAssignableToGenericType(type)).InstancePerDependency();
        }
    }
}
