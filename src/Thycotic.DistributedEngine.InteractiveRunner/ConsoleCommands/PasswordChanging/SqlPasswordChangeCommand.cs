using System;
using System.Collections.Generic;
using System.Linq;
using Thycotic.AppCore.Federator;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Heartbeat.Request;
using Thycotic.Messages.Heartbeat.Response;
using Thycotic.Messages.PasswordChanging.Request;
using Thycotic.Messages.PasswordChanging.Response;

namespace Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands.PasswordChanging
{
    class SqlPasswordChangeCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(SqlPasswordChangeCommand));

        public override string Name
        {
            get { return "changepassword_sql"; }
        }

        public override string Area {
            get { return CommandAreas.TestFunctions; }
        }

        public override string Description
        {
            get { return "Posts a SQL Change Password message to the exchange"; }
        }

        public SqlPasswordChangeCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;


            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                string server;
                string username;
                string password;
                string newPassword;
                if (!parameters.TryGet("server", out server)) return;
                if (!parameters.TryGet("username", out username)) return;
                if (!parameters.TryGet("password", out password)) return;
                if (!parameters.TryGet("newpassword", out newPassword)) return;

                var message = new SecretChangePasswordMessage();
                message.PasswordInfoProvider = new GenericPasswordInfoProvider();
                message.PasswordInfoProvider.PasswordTypeName = "Thycotic.AppCore.Federator.SqlAccountFederator";
                message.PasswordInfoProvider.PasswordTypeId = 2;

                var itemValues = new Dictionary<string, string>();
                itemValues["server"] = server;
                itemValues["username"] = username;
                itemValues["password"] = password;

                message.PasswordInfoProvider.ItemValues = itemValues;
                message.PasswordInfoProvider["server"] = server;
                message.PasswordInfoProvider["username"] = username;
                message.PasswordInfoProvider["password"] = password;
                message.PasswordInfoProvider.NewPassword = newPassword;
                try
                {

                    var response = _bus.BlockingPublish<SecretChangePasswordResponse>(exchangeNameProvider.GetCurrentExchange(), message, 30);
                    if (!response.Success)
                    {
                        _log.Error(response.StatusMessages.First());
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Password Change failed", ex);
                }


                _log.Info("Posting completed");

            };
        }
    }
}
