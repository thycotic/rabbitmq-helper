using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.CLI.OS;
using Thycotic.Logging;
using Thycotic.Utility.IO;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class UninstallPriorRabbitMqCommand : CommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof (UninstallPriorRabbitMqCommand));

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Uninstalls prior installation of RabbitMq"; }
        }

        public UninstallPriorRabbitMqCommand()
        {

            Action = parameters =>
            {
                var executablePath = InstallationConstants.RabbitMq.UninstallerPath;

                if (!File.Exists(executablePath))
                {
                    _log.Debug("No uninstaller found");
                    return 0;
                }

                var externalProcessRunner = new ExternalProcessRunner();

                var directoryInfo = new FileInfo(executablePath);
                var workingPath = directoryInfo.DirectoryName;

                const string silent = "/S";

                _log.Info("Uninstalling prior version of RabbitMq");

                externalProcessRunner.Run(executablePath, workingPath, silent);

                #region Hack
                if (Directory.Exists(InstallationConstants.RabbitMq.BinPath))
                {
                    //rabbit mq uninstaller seems to be async so we need to monitor the install directory until it's empty
                    while (Directory.Exists(InstallationConstants.RabbitMq.BinPath) && Directory.EnumerateFiles(InstallationConstants.RabbitMq.BinPath).Any())
                    {
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    }

                    //one last wait for system to release resources
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }

                #endregion

                var directoryCleaner = new DirectoryCleaner();

                directoryCleaner.Clean(InstallationConstants.RabbitMq.InstallPath);

                directoryCleaner.Clean(InstallationConstants.RabbitMq.ConfigurationPath);

                return 0;
            };
        }
    }
}