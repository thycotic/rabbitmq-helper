using System;
using System.IO;
using Thycotic.CLI;
using Thycotic.CLI.OS;
using Thycotic.Logging;
using Thycotic.Utility.Reflection;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class InstallRabbitMqCommand : ConsoleCommandBase
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (InstallRabbitMqCommand));

        public override string Name
        {
            get { return "installRabbitMq"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Installs RabbitMq"; }
        }

        public InstallRabbitMqCommand()
        {

            Action = parameters =>
            {
                var rabbitMqBase = Environment.GetEnvironmentVariable("RABBITMQ_BASE");

                if (rabbitMqBase != InstallationConstants.RabbitMq.ConfigurationPath)
                {
                    _log.Warn(string.Format("RABBITMQ_BASE is set to {0}", rabbitMqBase));
                    throw new ApplicationException("The RABBITMQ_BASE environmental variable is not set correctly");
                }

                var executablePath = DownloadRabbitMqCommand.RabbitMqInstallerPath;

                if (!File.Exists(executablePath))
                {
                    _log.Debug("No installer found");
                    return 0;
                }

                var externalProcessRunner = new ExternalProcessRunner
                {
                    EstimatedProcessDuration = TimeSpan.FromMinutes(1)
                };


                var assemblyEntryPointProvider = new AssemblyEntryPointProvider();

                var workingPath = assemblyEntryPointProvider.GetAssemblyDirectory(typeof(Program));

                const string silent = "/S";

                _log.Info("Installing RabbitMq, please wait...");

                externalProcessRunner.Run(executablePath, workingPath, silent);

                return 0;
            };
        }
    }
}
