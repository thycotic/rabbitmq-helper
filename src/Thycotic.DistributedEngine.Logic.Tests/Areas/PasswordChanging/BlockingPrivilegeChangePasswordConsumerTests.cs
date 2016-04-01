using System.Runtime.Remoting.Messaging;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.Logic.Areas.PasswordChanging;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.DE.Areas.PasswordChanging.Request;
using Thycotic.PasswordChangers.ActiveDirectory;
using Thycotic.PasswordChangers.MySql;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.PasswordChanging
{
    [TestFixture]
    [Ignore("This is just here for dev testing. -MW")]
    public class BlockingPrivilegeChangePasswordConsumerTests
    {
        [Test]
        public void ShouldChangeADPassword()
        {
            CallContext.FreeNamedDataSlot("log4net.Util.LogicalThreadContextProperties");
            
            var responseBus = Substitute.For<IResponseBus>();

            var consumer = new BlockingPrivilegeChangePasswordConsumer(responseBus);
            var domain = "testparent.thycotic.com";

            var message = new BlockingPrivilegedPasswordChangeMessage();
            message.OperationInfo = new ActiveDirectoryPrivilegedChangeInfo()
            {
                PrivilegedDomain = domain,
                PrivilegedUserName = "MrMittens",
                PrivilegedPassword = "Password1",
                TargetDomain = domain,
                TargetUserName = "ResetMe",
                TargetNewPassword = "Yagni12#"
            };
            var response = consumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(response.Success);

            var messageBack = new BlockingPrivilegedPasswordChangeMessage();
            messageBack.OperationInfo = new ActiveDirectoryPrivilegedChangeInfo()
            {
                PrivilegedDomain = domain,
                PrivilegedUserName = "MrMittens",
                PrivilegedPassword = "Password1",
                TargetDomain = domain,
                TargetUserName = "ResetMe",
                TargetNewPassword = "Password1"
            };
            response = consumer.Consume(CancellationToken.None, messageBack);

            Assert.IsTrue(response.Success);

            CallContext.FreeNamedDataSlot("log4net.Util.LogicalThreadContextProperties");
        }

        [Test]
        public void ShouldReturnErrorOnError()
        {
            var responseBus = Substitute.For<IResponseBus>();

            var consumer = new BlockingPrivilegeChangePasswordConsumer(responseBus);

            var message = new BlockingPrivilegedPasswordChangeMessage();
            message.OperationInfo = new ActiveDirectoryPrivilegedChangeInfo()
            {
                PrivilegedDomain = "testparent.thycotic.com",
                PrivilegedUserName = "MrMittens",
                PrivilegedPassword = "Bassword1",
                TargetDomain = "testparent.thycotic.com",
                TargetUserName = "ResetMe",
                TargetNewPassword = "Yagni12#"
            };
            var response = consumer.Consume(CancellationToken.None, message);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors[0].Message.IndexOf("Privileged Account The user name or password is incorrect.") == 0);
        }

        [Test]
        public void ShouldChangeMySQLPassword()
        {
            var responseBus = Substitute.For<IResponseBus>();

            var consumer = new BlockingPrivilegeChangePasswordConsumer(responseBus);

            var message = new BlockingPrivilegedPasswordChangeMessage();
            message.OperationInfo = TestInfo;

            var response = consumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(response.Success);

            var messageBack = new BlockingPrivilegedPasswordChangeMessage();
            TestInfo.TargetNewPassword = "password1";
            messageBack.OperationInfo = TestInfo;
            response = consumer.Consume(CancellationToken.None, messageBack);

            Assert.IsTrue(response.Success);
        }

        public MySqlAccountPrivilegedChangeInfo TestInfo
        {
            get
            {
                return new MySqlAccountPrivilegedChangeInfo
                {
                    Server = "centostestserver.testparent.thycotic.com",
                    Port = "3306",
                    ApplicationPath = "..\\..\\..\\assemblies",
                    TargetUserName = "ssluser_unittest",
                    TargetNewPassword = "newrandompassword",
                    PrivilegedUserName = "test",
                    PrivilegedPassword = "test"

                };
            }
        }
    }
}
