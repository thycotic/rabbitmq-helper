using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory.Response;
using Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.Areas.ActiveDirectory;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.ActiveDirectory
{
    [TestFixture]
    public class ADSyncRequestConsumerTests
    {
        [Test]
        public void TestName()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var adSyncRequestConsumer = new ADSyncRequestConsumer(responseBus);

            adSyncRequestConsumer.Consume(new ADSyncMessage());

            responseBus.Received().Execute(Arg.Any<ADSyncBatchResponse>());
        } 
    }
}