using System;
using System.IO;
using System.Threading.Tasks;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.Utility.IO;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    internal class UninstallPriorErlangCommand : CommandBase, IImmediateCommand
    {

        private readonly ILogWriter _log = Log.Get(typeof(UninstallPriorErlangCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }


        public override string[] Aliases
        {
            get
            {
                return new[]
                {
                    "ue"
                };
            }
            set { }
        }

        public override string Description
        {
            get { return "Uninstalls prior installation of Erlang"; }
        }

        public UninstallPriorErlangCommand()
        {

            Action = parameters =>
            {
                _log.Info("Uninstalling prior version of Erlang");

                var executablePath = InstallationConstants.Erlang.UninstallerPath;

                if (!File.Exists(executablePath))
                {
                    _log.Info("No uninstaller found");

                    CleanUpFolders();

                    return 0;
                }

                var externalProcessRunner = new ExternalProcessRunner();

                var directoryInfo = new FileInfo(executablePath);
                var workingPath = directoryInfo.DirectoryName;

                const string silent = "/S";

                externalProcessRunner.Run(executablePath, workingPath, silent);

                try
                {

                    const string erlandProcessKill = " /F /IM epmd.exe";
                    externalProcessRunner.Run("taskkill", workingPath, erlandProcessKill);
                }
                catch (Exception ex)
                {
                    _log.Warn("Failed to terminate erlang process. Clean removal might fail", ex);
                }

                _log.Info("Waiting for Erlang process to exit...");
                Task.Delay(TimeSpan.FromSeconds(15)).Wait();

                CleanUpFolders();

                _log.Info("Uninstallation process completed");

                return 0;

            };
        }

        private void CleanUpFolders()
        {
            var directoryCleaner = new DirectoryCleaner();

            try
            {
                directoryCleaner.Clean(InstallationConstants.Erlang.InstallPath);
            }
            catch (Exception ex)
            {
                _log.Warn("Failed to clean installation path. Clean removal might fail", ex);
            }
        }
    }
}