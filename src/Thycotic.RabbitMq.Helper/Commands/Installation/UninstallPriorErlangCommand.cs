using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thycotic.CLI.Commands;
using Thycotic.CLI.OS;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;
using Thycotic.Utility.IO;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    internal class UninstallPriorErlangCommand : CommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(UninstallPriorErlangCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
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

                #region Hack
                if (Directory.Exists(InstallationConstants.Erlang.InstallPath))
                {
                    //rabbit mq uninstaller seems to be async so we need to monitor the install directory until it's empty
                    while (Directory.Exists(InstallationConstants.Erlang.InstallPath) && Directory.EnumerateFiles(InstallationConstants.Erlang.InstallPath).Any())
                    {
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    }

                    //one last wait for system to release resources
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }
                #endregion


                var directoryCleaner = new DirectoryCleaner();

                directoryCleaner.Clean(InstallationConstants.Erlang.InstallPath);

                return 0;

            };
        }
    }
}