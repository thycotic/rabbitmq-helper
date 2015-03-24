using Autofac;
using NSubstitute;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.DistributedEngine.EngineToServer;
using Thycotic.DistributedEngine.EngineToServerCommunication;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    class LoopbackIoCConfigurator : IoCConfigurator
    {
        //use static since channel can be reinitialized through IoC
        public static IEngineToServerCommunicationWcfService LoopbackEngineToServerChannel = new LoopbackEngineToServerChannel();

        protected override void RegisterPreAuthorization(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var connection = Substitute.For<IEngineToServerConnection>();

                connection.OpenChannel().Returns(info => LoopbackEngineToServerChannel);

                return connection;
            }).As<IEngineToServerConnection>();

            builder.RegisterType<EngineConfigurationBus>().AsImplementedInterfaces().SingleInstance();
        }

        
    }
}
