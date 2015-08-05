using System;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Commands.Installation;
using Thycotic.RabbitMq.Helper.Management;
using Thycotic.Utility.OS;

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
                if (!parameters.TryGet("rabbitMqUsername", out username))
                {
                    _log.Error("RabbitMq username is required");
                    return 1;
                }
                if (!parameters.TryGet("rabbitMqPw", out password))
                {
                    _log.Error("RabbitMq password is required");
                    return 1;
                }

                var externalProcessRunner = new ExternalProcessRunner
                {
                    EstimatedProcessDuration = TimeSpan.FromSeconds(15)
                };

                _log.Info(string.Format("Adding user {0}", username));

                var parameters2 = string.Format("add_user {0} {1}", username, password);

                try
                {

                    externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters2);
                }
                catch (Exception ex)
                {
                    _log.Warn("Failed to create user. Manual creation might be necessary", ex);
                }

                _log.Info(string.Format("Granting permissions to user {0}", username));

                parameters2 = string.Format("set_permissions -p / {0} \".*\" \".*\" \".*\"", username);

                try
                {
                    externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters2);
                }
                catch (Exception ex)
                {
                    _log.Warn("Failed to grant permissions to user. Manual grant might be necessary", ex);
                }

                return 0;

            };
        }
    }
}