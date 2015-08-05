using System;
using System.Diagnostics;
using System.IO;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Commands.Installation;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.Commands.Management
{
    internal class EnableRabbitManagementPlugin : ManagementConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Area
        {
            get { return CommandAreas.Management; }
        }

        public override string Description
        {
            get { return "Enables the RabbitMq management plugin (https://www.rabbitmq.com/management.html)"; }
        }

        public EnableRabbitManagementPlugin()
        {
            const string executable = "rabbitmq-plugins.bat";
            var pluginsExecutablePath = Path.Combine(InstallationConstants.RabbitMq.BinPath, executable);

            Action = parameters =>
            {
                var externalProcessRunner = new ExternalProcessRunner
                {
                    EstimatedProcessDuration = TimeSpan.FromSeconds(15)
                };

                const string parameters2 = "enable rabbitmq_management";

                externalProcessRunner.Run(pluginsExecutablePath, WorkingPath, parameters2);


                _log.Info("Opening management console");
                Process.Start("http://localhost:15672/");

                return 0;
            };
        }
    }
}