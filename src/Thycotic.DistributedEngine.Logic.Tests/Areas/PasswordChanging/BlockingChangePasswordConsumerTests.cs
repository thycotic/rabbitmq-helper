using System;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.Logic.Areas.PasswordChanging;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.DE.Areas.PasswordChanging.Request;
using Thycotic.PasswordChangers.ActiveDirectory;
using Thycotic.PasswordChangers.Commands;
using Thycotic.PasswordChangers.ODBC;
using Thycotic.PasswordChangers.SSH;
using Thycotic.PasswordChangers.Telnet;
using Thycotic.SharedTypes.General;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.PasswordChanging
{
    [TestFixture]
    [Ignore("This is just here for dev testing. -MW")]
    public class BlockingChangePasswordConsumerTests
    {
        [Test]
        public void ShouldChangeADPassword()
        {
            var responseBus = Substitute.For<IResponseBus>();

            var consumer = new BlockingChangePasswordConsumer(responseBus);

            var message = new BlockingPasswordChangeMessage();
            message.OperationInfo = new ActiveDirectoryBasicChangeInfo()
            {
                Domain = "Testparent.Thycotic.com",
                UserName = "ResetMe",
                CurrentPassword = "Password1",
                NewPassword = "Yagni12#"
            };
            var response = consumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(response.Success);

            var messageBack = new BlockingPasswordChangeMessage();
            messageBack.OperationInfo = new ActiveDirectoryBasicChangeInfo()
            {
                Domain = "Testparent.Thycotic.com",
                UserName = "ResetMe",
                CurrentPassword = "Yagni12#",
                NewPassword = "Password1"
            };
            response = consumer.Consume(CancellationToken.None, messageBack);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public void ShouldReturnErrorOnError()
        {
            var responseBus = Substitute.For<IResponseBus>();

            var consumer = new BlockingChangePasswordConsumer(responseBus);

            var message = new BlockingPasswordChangeMessage();
            message.OperationInfo = new ActiveDirectoryBasicChangeInfo()
            {
                Domain = "Testparent.Thycotic.com",
                UserName = "ResetMe",
                CurrentPassword = "Password2",
                NewPassword = "Yagni12#"
            };
            var response = consumer.Consume(CancellationToken.None, message);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("The specified network password is not correct. (86)", response.Errors[0].Message);
        }

        [Test]
        public void ShouldChangeSshPassword()
        {
            var responseBus = Substitute.For<IResponseBus>();

            var consumer = new BlockingChangePasswordConsumer(responseBus);

            var message = new BlockingPasswordChangeMessage();
            var username = "unittestuser";
            var currentPassword = "password1";
            var newPassword = "Yagni12#";
            message.OperationInfo = new SshAccountBasicChangeInfo()
            {
               Server = "bsdradiusserver.testparent.thycotic.com",
               ResetUserName = username,
               VerifyUserName = username,
            CurrentPassword = currentPassword,
            NewPassword = newPassword,
               EnforceFipsCompliance = false,
               HostPublicKeyDigest = null,
               LineEnding = LineEnding.NewLine,
               Port = 22,
               ResetCommandSet = new GenericCommandSet
               {
                   TerminalCommands =
                    {
                        new TerminalCommand("passwd", "first"),
                        new TerminalCommand(currentPassword, "current"),
                        new TerminalCommand(newPassword, "new"),
                        new TerminalCommand(newPassword, "confirm")
                    }
               },
               SshKey = null,
               SshKeyPassphrase = null,
               ValidateHostKey = false
            };
            var response = consumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(response.Success);

            var messageBack = new BlockingPasswordChangeMessage();
            messageBack.OperationInfo = new SshAccountBasicChangeInfo()
            {
                Server = "bsdradiusserver.testparent.thycotic.com",
                ResetUserName = username,
                VerifyUserName = username,
                CurrentPassword = newPassword,
                NewPassword = currentPassword,
                EnforceFipsCompliance = false,
                HostPublicKeyDigest = null,
                LineEnding = LineEnding.NewLine,
                Port = 22,
                ResetCommandSet = new GenericCommandSet
                {
                    TerminalCommands =
                    {
                        new TerminalCommand("passwd", "first"),
                        new TerminalCommand(newPassword, "current"),
                        new TerminalCommand(currentPassword, "new"),
                        new TerminalCommand(currentPassword, "confirm")
                    }
                },
                SshKey = null,
                SshKeyPassphrase = null,
                ValidateHostKey = false
            };
            response = consumer.Consume(CancellationToken.None, messageBack);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public void ShouldChangeODBCPassword()
        {
            var responseBus = Substitute.For<IResponseBus>();

            var consumer = new BlockingChangePasswordConsumer(responseBus);

            var username = "test";
            var currentPassword = "test";
            var newPassword = "test2";

            var message = new BlockingPasswordChangeMessage();
            message.OperationInfo = new OdbcBasicChangeInfo()
            {
                ConnectionString =
                    string.Format("Driver={{SQL Server}};Server=localhost;Database=master;Uid={0};Pwd={1};", username,
                        currentPassword),
                Username = username,
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                TranslatedParameters = new[]
                {
                    new Tuple<string, string>("@CURRENTPASSWORD", currentPassword),
                    new Tuple<string, string>("@NEWPASSWORD", newPassword)
                },
                TranslatedCommandText = "EXEC sp_password ?, ?;"
            };

            var response = consumer.Consume(CancellationToken.None, message);

            Assert.IsTrue(response.Success);

            var messageBack = new BlockingPasswordChangeMessage();
            messageBack.OperationInfo = new OdbcBasicChangeInfo()
            {
                ConnectionString =
                    string.Format("Driver={{SQL Server}};Server=localhost;Database=master;Uid={0};Pwd={1};", username,
                        newPassword),
                Username = username,
                CurrentPassword = newPassword,
                NewPassword = currentPassword,
                TranslatedParameters = new[]
                {
                    new Tuple<string, string>("@CURRENTPASSWORD", newPassword),
                    new Tuple<string, string>("@NEWPASSWORD", currentPassword)
                },
                TranslatedCommandText = "EXEC sp_password ?, ?;"
            };
            response = consumer.Consume(CancellationToken.None, messageBack);

            Assert.IsTrue(response.Success);
        }
    }
}
