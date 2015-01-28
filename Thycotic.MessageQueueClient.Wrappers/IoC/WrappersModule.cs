using Autofac;
using Thycotic.Logging;
using Module = Autofac.Module;

namespace Thycotic.MessageQueueClient.Wrappers.IoC
{
    public class WrappersModule : Module
    {
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
