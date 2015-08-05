using System;
using System.Diagnostics;
using System.IO;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Commands.Installation;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.Commands.Management
{
    internal class EnableRabbitManagementPluginCommand : ManagementConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(EnableRabbitManagementPluginCommand));

        public override string Area
        {
            get { return CommandAreas.Management; }
        }

        public override string Description
        {
            get { return "Enables the RabbitMq management plugin (https://www.rabbitmq.com/management.html)"; }
        }

        public EnableRabbitManagementPluginCommand()
        {
            //we have to use local host because guest account does not work under FQDN
            const string pluginUrl = "http://localhost:15672/";
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

                _log.Info(string.Format("Opening management console at {0}", pluginUrl));
                Process.Start(pluginUrl);

                return 0;
            };
        }
    }
}