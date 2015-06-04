using System.IO;
using Thycotic.CLI;
using Thycotic.CLI.OS;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class UninstallPriorRabbitMqCommand : ConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof (UninstallPriorRabbitMqCommand));

        public override string Name
        {
            get { return "uninstallPriorRabbitMq"; }
        }

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

                externalProcessRunner.Run(executablePath, workingPath, silent);

                //if (directoryInfo.Directory != null)
                //{
                //    directoryInfo.Directory.Delete(true);
                //}

                return 0;
            };
        }
    }
}