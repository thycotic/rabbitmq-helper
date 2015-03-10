using Autofac;
using Thycotic.Logging;
using Module = Autofac.Module;

namespace Thycotic.MessageQueue.Client.Wrappers.IoC
{
    /// <summary>
    /// Module to register wrappers and their factory
    /// </summary>
    public class WrappersModule : Module
    {
        private readonly ILogWriter _log = Log.Get(typeof(WrappersModule));
        
        /// <summary>
        /// Loads wrappers.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing consumer wrappers...");

            builder.RegisterGeneric(typeof(BasicConsumerWrapper<,>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(BlockingConsumerWrapper<,,>)).InstancePerDependency();

            //register as self too when we want to clean it up explicitly
            builder.RegisterType<ConsumerWrapperFactory>().As<IStartable>().AsSelf().SingleInstance();
        }
    }
}
