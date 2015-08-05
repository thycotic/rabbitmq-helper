using System;
using System.IO;
using System.Threading.Tasks;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.Utility.OS;
using Thycotic.Utility.Reflection;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    internal class InstallRabbitMqCommand : CommandBase, IImmediateCommand
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (InstallRabbitMqCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
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

                _log.Info("Installing RabbitMq...");

                externalProcessRunner.Run(executablePath, workingPath, silent);

                _log.Info("Waiting for RabbitMq process to start...");
                Task.Delay(TimeSpan.FromSeconds(10)).Wait();

                return 0;
            };
        }
    }
}
