using System.Threading;
using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.Logic.Areas.Verify;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.DE.Areas.Verify.Request;
using Thycotic.PasswordChangers.ActiveDirectory;
using Thycotic.PasswordChangers.ODBC;
using Thycotic.PasswordChangers.SSH;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.Verify
{
    [TestFixture]
    [Ignore("This is just here for dev testing. -MW")]
    public class VerifyPasswordConsumerTests
    {
        [Test]
        public void ShouldVerifyAdPassword()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var verifyCredentialsInfo = new ActiveDirectoryVerifyInfo();
            verifyCredentialsInfo.Domain = "Testparent.Thycotic.Com";
            verifyCredentialsInfo.UserName = "MrMittens";
            verifyCredentialsInfo.Password = "Password1";

            var message = new VerifyPasswordMessage()
            {
                VerifyCredentialsInfo = verifyCredentialsInfo
            };

            var verifyPasswordConsumer = new VerifyPasswordConsumer(responseBus);

            var result = verifyPasswordConsumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldErrorOnBadCredentials()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var verifyCredentialsInfo = new ActiveDirectoryVerifyInfo();
            verifyCredentialsInfo.Domain = "Testparent.Thycotic.Com";
            verifyCredentialsInfo.UserName = "MrMutton";
            verifyCredentialsInfo.Password = "Password1";

            var message = new VerifyPasswordMessage()
            {
                VerifyCredentialsInfo = verifyCredentialsInfo
            };

            var verifyPasswordConsumer = new VerifyPasswordConsumer(responseBus);

            var result = verifyPasswordConsumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(!result.Success);
            Assert.AreEqual("Logon failure: unknown user name or bad password.", result.Errors[0].Message.ToString());
        }

        [Test]
        public void ShouldVerifySSHPassword()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var verifyCredentialsInfo = new SshAccountVerifyInfo();
            verifyCredentialsInfo.Server = "bsdradiusserver.testparent.thycotic.com";
            verifyCredentialsInfo.UserName = "unittestuser";
            verifyCredentialsInfo.Password = "password1";
            verifyCredentialsInfo.Port = 22;

            var message = new VerifyPasswordMessage()
            {
                VerifyCredentialsInfo = verifyCredentialsInfo
            };

            var verifyPasswordConsumer = new VerifyPasswordConsumer(responseBus);

            var result = verifyPasswordConsumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldVerifyODBCPassword()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var verifyCredentialsInfo = new OdbcVerifyInfo();

            verifyCredentialsInfo.ConnectionString = string.Format("Driver={{SQL Server}};Server=localhost;Database=master;Uid={0};Pwd={1};", "test", "test");

            var message = new VerifyPasswordMessage()
            {
                VerifyCredentialsInfo = verifyCredentialsInfo
            };

            var verifyPasswordConsumer = new VerifyPasswordConsumer(responseBus);

            var result = verifyPasswordConsumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(result.Success);
        }
    }
}