using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.Logic.Areas.Heartbeat;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.Heartbeat.Request;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.Heartbeat
{
    [TestFixture]
    public class SecretHeartbeatConsumerTests
    {
        [Test]
        public void ShouldHandleExceptionDueToNonMatchingVerifyCredentialsInfo()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var verifyCredentialsInfo = Substitute.For<IVerifyCredentialsInfo>();
            var consumer = new SecretHeartbeatConsumer(responseBus);

            var message = new SecretHeartbeatMessage();
            message.VerifyCredentialsInfo = verifyCredentialsInfo;
            consumer.Consume(message);

            responseBus.Received().ExecuteAsync(Arg.Is<SecretHeartbeatResponse>(x => x.Status == OperationStatus.Unknown));
        }
    }
}
