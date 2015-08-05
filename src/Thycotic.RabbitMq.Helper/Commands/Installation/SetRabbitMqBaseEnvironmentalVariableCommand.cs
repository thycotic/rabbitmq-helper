using System;
using System.Linq;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    internal class SetRabbitMqBaseEnvironmentalVariableCommand : CommandBase, IImmediateCommand
    {
        public const string RabbitMqBaseEnvironmentalVariableName = "RABBITMQ_BASE";

        private readonly ILogWriter _log = Log.Get(typeof (SetRabbitMqBaseEnvironmentalVariableCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Sets the RABBITMQ_BASE environmental variable"; }
        }

        public SetRabbitMqBaseEnvironmentalVariableCommand()
        {

            Action = parameters =>
            {
                _log.Info("Setting RabbitMq environmental variables for RabbitMq");

                var targets = new[]
                {
                    EnvironmentVariableTarget.Machine,
                    EnvironmentVariableTarget.Process
                };

                targets.ToList().ForEach(t =>
                    Environment.SetEnvironmentVariable(RabbitMqBaseEnvironmentalVariableName,
                        InstallationConstants.RabbitMq.ConfigurationPath, t));
                


                return 0;
            };
        }
    }
}