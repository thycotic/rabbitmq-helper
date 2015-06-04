using System.IO;
using Thycotic.CLI;
using Thycotic.CLI.OS;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class UninstallPriorErlangCommand : ConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(UninstallPriorErlangCommand));

        public override string Name
        {
            get { return "uninstallPriorErlang"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Uninstalls prior installation of Erlang"; }
        }

        public UninstallPriorErlangCommand()
        {

            Action = parameters =>
            {
                var executablePath = InstallationConstants.Erlang.UninstallerPath;

                if (!File.Exists(executablePath))
                {
                    _log.Debug("No uninstaller found");
                    return 0;
                }

                var externalProcessRunner = new ExternalProcessRunner();

                var directoryInfo = new FileInfo(executablePath);
                var workingPath = directoryInfo.DirectoryName;

                const string silent = "/S";

                _log.Info("Uninstalling prior version of Erlang");

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