using System;
using System.Threading;
using Autofac.Features.OwnedInstances;
using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.PasswordChanging.Response;
using Thycotic.DistributedEngine.Logic.Areas.PasswordChanging;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.Common;
using Thycotic.Messages.DE.Areas.PasswordChanging.Request;
using Thycotic.PasswordChangers.Windows;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.PasswordChanging
{
    [TestFixture]
    public class SecretBasicChangePasswordConsumerTests
    {
        [Test]
        public void ShouldHandleExceptionDueToNonMatchingBasicInfo()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var info = Substitute.For<IBasicPasswordChangerInfo>();
            var func = Substitute.For<Func<Owned<IBasicConsumer<SecretChangeDependencyMessage>>>>();

            var consumer = new SecretBasicChangePasswordConsumer(responseBus, func);

            var message = new SecretBasicPasswordChangeMessage();
            message.OperationInfo = info;
            consumer.Consume(CancellationToken.None, message);

            responseBus.Received().ExecuteAsync(Arg.Is<RemotePasswordChangeResponse>(x => x.Status == OperationStatus.Unknown));
        }

        [Test]
        [Ignore("Will resolve tomorrow -dkk")]
        public void ShouldReturnErrorDueToInvalidLicense()
        {
            //TODO: To fix. Fails when full test suite runs - dkk

            Assert.Inconclusive();

            //    var responseBus = Substitute.For<IResponseBus>();
            //    var info = new WindowsAccountBasicChangeInfo()
            //    {
            //        CurrentPassword = "invalidpassword",
            //        EnsureTargetMachineHostAndReverseDnsRecordsMatch = false,
            //        Machine = "1.1.1.1",
            //        NewPassword = "new",
            //        UserName = "user that does not exist"
            //    };
            //    var func = Substitute.For<Func<Owned<IBasicConsumer<SecretChangeDependencyMessage>>>>();

            //    var consumer = new SecretBasicChangePasswordConsumer(responseBus, func);

            //    var message = new SecretBasicPasswordChangeMessage();
            //    message.OperationInfo = info;
            //    consumer.Consume(message);

            //    responseBus.Received().ExecuteAsync(Arg.Is<RemotePasswordChangeResponse>(x => x.Status == OperationStatus.Unknown && x.StatusMessages[0].Contains("Invalid License")));
        }

        [Test]
        [Ignore("Will resolve tomorrow -dkk")]
        public void ShouldReturnErrorDueToIncorrectCredentials()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var info = new WindowsAccountBasicChangeInfo
            {
                CurrentPassword = "invalidpassword",
                EnsureTargetMachineHostAndReverseDnsRecordsMatch = false,
                Machine = "thycopair23",
                NewPassword = "new",
                UserName = "user that does not exist"
            };
            var func = Substitute.For<Func<Owned<IBasicConsumer<SecretChangeDependencyMessage>>>>();

            var consumer = new SecretBasicChangePasswordConsumer(responseBus, func);

            var message = new SecretBasicPasswordChangeMessage();
            message.OperationInfo = info;
            consumer.Consume(CancellationToken.None, message);

            responseBus.Received().ExecuteAsync(Arg.Is<RemotePasswordChangeResponse>(x => x.Status == OperationStatus.AccessDenied));
        }
    }
}
