using System;
using System.Diagnostics;
using System.IO;
using Thycotic.CLI;
using Thycotic.CLI.OS;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;

namespace Thycotic.RabbitMq.Helper.Management
{
    internal class EnableRabbitManagementPlugin : ManagementConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Name
        {
            get { return "enableRabbitMqManagementPlugin"; }
        }

        public override string Area
        {
            get { return "Management"; }
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