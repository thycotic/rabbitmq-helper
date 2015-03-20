using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.DistributedEngine.EngineToServer;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.DistributedEngine.Security;
using Thycotic.Logging;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    class LoopbackIoCConfigurator : IoCConfigurator
    {
        private static readonly IEngineConfigurationBus EngineConfigurationBus = new LoopbackEngineConfigurationBus();

        protected override void RegisterPreAuthorization(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var connection = Substitute.For<IEngineToServerConnection>();

                var channel = Substitute.For<IEngineToServerChannel>();

                channel.When(call => call.BasicPublish(Arg.Any<IBasicConsumable>())).Do(callInfo => Task.Factory.StartNew(() =>
                {
                    var consumable = callInfo.Args().First();

                    if (consumable is EnginePingRequest)
                    {
                        //don't worry about those success or printing any output
                    }
                    else
                    {
                        ConsumerConsole.WriteLine(
                            string.Format("Intercepted basic publish call to server for {0}", consumable),
                            ConsoleColor.Blue);
                    }
                }));

                channel.When(call => call.BlockingPublish<IBlockingConsumable>(Arg.Any<IBlockingConsumable>())).Do(callInfo =>Task.Factory.StartNew(() =>
                {
                    var consumable = callInfo.Args().First();

                    ConsumerConsole.WriteLine(string.Format("Intercepted blocking publish call to server for {0}", consumable), ConsoleColor.Blue);
                }));

                connection.OpenChannel(Arg.Any<IObjectSerializer>(), Arg.Any<IEngineToServerEncryptor>())
                    .Returns(channel);

                return connection;
            }).As<IEngineToServerConnection>();

            builder.Register(context => EngineConfigurationBus).As<IEngineConfigurationBus>().SingleInstance();
        }
    }
}
