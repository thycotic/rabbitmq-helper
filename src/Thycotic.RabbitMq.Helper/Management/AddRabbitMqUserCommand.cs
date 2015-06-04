using System;
using System.IO;
using Thycotic.CLI.OS;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;
using Thycotic.Utility.Reflection;

namespace Thycotic.RabbitMq.Helper.Management
{
    internal class AddRabbitMqUserCommand : ManagementConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Name
        {
            get { return "addRabbitMqUserCommand"; }
        }

        public override string Area
        {
            get { return "Management"; }
        }

        public override string Description
        {
            get { return "Adds a basic user to RabbitMq"; }
        }

        public AddRabbitMqUserCommand()
        {

            Action = parameters =>
            {
                bool skipUserCreate;
                if (parameters.TryGetBoolean("skipUserCreate", out skipUserCreate) && skipUserCreate)
                {
                    _log.Info("Skipping user creation");
                    return 0;
                }

                string username;
                string password;
                if (!parameters.TryGet("rabbitUsername", out username)) return 1;
                if (!parameters.TryGet("rabbitPw", out password)) return 1;
                
                var externalProcessRunner = new ExternalProcessRunner
                {
                    EstimatedProcessDuration = TimeSpan.FromSeconds(15)
                };

                var parameters2 = string.Format("add_user {0} {1}", username, password);

                externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters2);

                return 0;

            };
        }
    }
}