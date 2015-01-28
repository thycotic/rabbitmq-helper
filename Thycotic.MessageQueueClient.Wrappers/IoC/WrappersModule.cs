using System;
using System.Reflection;
using Autofac;
using Thycotic.Logging;
using Thycotic.Utility;
using Module = Autofac.Module;

namespace Thycotic.MessageQueueClient.Wrappers.IoC
{
    public class WrappersModule : Module
    {
        private readonly ILogWriter _log = Log.Get(typeof(WrappersModule));

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            using (LogContext.Create("Initializing consumer wrappers..."))
            {

                builder.RegisterGeneric(typeof (SimpleConsumerWrapper<,>)).InstancePerDependency();
                builder.RegisterGeneric(typeof (RpcConsumerWrapper<,,>)).InstancePerDependency();
                builder.RegisterType<ConsumerWrapperFactory>().As<IStartable>().SingleInstance();
            }
        }

    }
}
