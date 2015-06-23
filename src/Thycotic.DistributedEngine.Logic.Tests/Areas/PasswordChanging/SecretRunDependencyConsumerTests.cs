using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Dependency.Response;
using Thycotic.DistributedEngine.Logic.Areas.PasswordChanging;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.PasswordChanging.Request;
using Thycotic.SharedTypes.Dependencies;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.PasswordChanging
{
    [TestFixture]
    public class SecretRunDependencyConsumerTests
    {
        [Test]
        public void ShouldDoNothingWhenNoDependenciesPassedIn()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var nameProvider = Substitute.For<IExchangeNameProvider>();

            var consumer = new SecretRunDependenciesConsumer(responseBus, nameProvider);

            var message = new SecretChangeDependencyMessage();
            message.DependencyChangeInfos = new IDependencyChangeInfo[0];
            consumer.Consume(message);

            responseBus.DidNotReceive().ExecuteAsync(Arg.Any<DependencyChangeResponse>());
        }

        [Test]
        public void ShouldFailWithInvalidLicense()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var nameProvider = Substitute.For<IExchangeNameProvider>();

            var consumer = new SecretRunDependenciesConsumer(responseBus, nameProvider);

            var message = new SecretChangeDependencyMessage();
            message.DependencyChangeInfos = new IDependencyChangeInfo[] {new ApplicationPoolChangeInfo
            {
                AccountDomainName = "testparent.thycotic.com",
                AccountUserName = "username",
                AccountPassword = "invalid password",
                MachineName = "invalid machine"
            }};
            consumer.Consume(message);

            responseBus.Received().ExecuteAsync(Arg.Is<DependencyChangeResponse>(x => !x.Success 
                && x.StatusMessages[0].Contains("Invalid License")));
        }        
    }
}
