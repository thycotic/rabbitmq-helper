using System;
using System.Linq;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    internal class SetErlangHomeEnvironmentalVariableCommand : CommandBase, IImmediateCommand
    {
        public const string ErlangHomeEnvironmentalVariableName = "ERLANG_HOME";

        private readonly ILogWriter _log = Log.Get(typeof(SetErlangHomeEnvironmentalVariableCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Sets the ERLANG_HOME environmental variable"; }
        }

        public SetErlangHomeEnvironmentalVariableCommand()
        {

            Action = parameters =>
            {
                _log.Info("Setting Erlang environmental variables");

                var targets = new[]
                {
                    EnvironmentVariableTarget.Machine,
                    EnvironmentVariableTarget.Process
                };

                targets.ToList().ForEach(t =>
                    Environment.SetEnvironmentVariable(ErlangHomeEnvironmentalVariableName,
                        InstallationConstants.Erlang.InstallPath, t));
                


                return 0;
            };
        }
    }
}