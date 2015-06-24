using System;
using Thycotic.CLI.OS;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Commands.Installation;
using Thycotic.RabbitMq.Helper.Installation;
using Thycotic.RabbitMq.Helper.Management;

namespace Thycotic.RabbitMq.Helper.Commands.Management
{
    internal class AddRabbitMqUserCommand : ManagementConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Area
        {
            get { return CommandAreas.Management; }
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
                if (!parameters.TryGet("rabbitMqUsername", out username)) return 1;
                if (!parameters.TryGet("rabbitMqPw", out password)) return 1;
                
                var externalProcessRunner = new ExternalProcessRunner
                {
                    EstimatedProcessDuration = TimeSpan.FromSeconds(15)
                };

                _log.Info(string.Format("Adding user {0}", username));

                var parameters2 = string.Format("add_user {0} {1}", username, password);

                externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters2);

                _log.Info(string.Format("Granting permissions to user {0}", username));

                parameters2 = string.Format("set_permissions -p / {0} \".*\" \".*\" \".*\"", username);

                externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters2);



                return 0;

            };
        }
    }
}