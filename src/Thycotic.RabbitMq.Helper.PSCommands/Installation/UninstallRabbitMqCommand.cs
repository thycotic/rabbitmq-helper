using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Thycotic.Utility.IO;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Uninstalls prior installation of RabbitMq
    /// </summary>
    /// <para type="synopsis">Uninstalls prior installation of RabbitMq</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Uninstall-Erlang</para>
    /// <para type="link">Install-Connector</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Uninstall-RabbitMq</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Uninstall, "RabbitMq")]
    public class UninstallRabbitMqCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteVerbose("Uninstalling prior version of RabbitMq");

            var executablePath = InstallationConstants.RabbitMq.UninstallerPath;

            if (!File.Exists(executablePath))
            {
                WriteVerbose("No uninstaller found");

                CleanUpFolders();

                return;
            }

            var externalProcessRunner = new ExternalProcessRunner();

            var directoryInfo = new FileInfo(executablePath);
            var workingPath = directoryInfo.DirectoryName;

            const string silent = "/S";

            externalProcessRunner.Run(executablePath, workingPath, silent);

            WriteVerbose("Waiting for RabbitMq process to exit...");
            if (Directory.Exists(InstallationConstants.RabbitMq.BinPath))
            {
                //rabbit mq uninstaller seems to be async so we need to monitor the install directory until it's empty
                while (Directory.Exists(InstallationConstants.RabbitMq.BinPath) &&
                       Directory.EnumerateFiles(InstallationConstants.RabbitMq.BinPath).Any())
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();

                //one last wait for system to release resources
                Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            }

            CleanUpFolders();

            WriteVerbose("Uninstallation process completed");
        }

        private void CleanUpFolders()
        {
            var directoryCleaner = new DirectoryCleaner();

            try
            {
                directoryCleaner.Clean(InstallationConstants.RabbitMq.InstallPath);
            }
            catch (Exception ex)
            {
                WriteWarning("Failed to clean installation path. Clean removal might fail: " + ex.Message);
            }

            try
            {
                directoryCleaner.Clean(InstallationConstants.RabbitMq.ConfigurationPath);
            }
            catch (Exception ex)
            {
                WriteWarning("Failed to clean configuration path. Clean removal might fail: " + ex.Message);
            }
        }
    }
}