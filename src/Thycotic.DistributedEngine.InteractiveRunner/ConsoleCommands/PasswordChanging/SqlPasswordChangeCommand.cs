using System.Collections.Generic;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;

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
                _log.Info("DEPRECATED - DOES NOTHING. Posting message to exchange");

                string server;
                string username;
                string password;
                string newPassword;
                if (!parameters.TryGet("server", out server)) return;
                if (!parameters.TryGet("username", out username)) return;
                if (!parameters.TryGet("password", out password)) return;
                if (!parameters.TryGet("newpassword", out newPassword)) return;

               

                var itemValues = new Dictionary<string, string>();
                itemValues["server"] = server;
                itemValues["username"] = username;
                itemValues["password"] = password;

               


                _log.Info("Posting completed");

            };
        }
    }
}
