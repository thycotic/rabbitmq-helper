using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.IO;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;
using Thycotic.RabbitMq.Helper.Logic.OS;

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
            var client = new RabbitMqBatCtlClient();
            if (client.Exists)
            {
                try
                {
                    WriteVerbose("Stopping RabbitMq");

                    client.HardStop();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to stop RabbitMq. Manual stop/uninstall might be necessary", ex);
                }
            }

            WriteVerbose("Un-installing prior versions of RabbitMq");

            var executablePaths = InstallationConstants.RabbitMq.UninstallerPaths;

            var externalProcessRunner = new ExternalProcessRunner();

            var shouldDeleteService = false;

            foreach (var executablePath in executablePaths)
            {
                var directoryInfo = new FileInfo(executablePath);
                var workingPath = directoryInfo.DirectoryName;

                if (!File.Exists(executablePath))
                {
                    //WriteVerbose("No uninstaller found at " + executablePath);

                    CleanUpFolders(workingPath);

                    continue;
                }

                shouldDeleteService = true;
                
                const string silent = "/S";

                externalProcessRunner.Run(executablePath, workingPath, silent);

                WriteVerbose("Waiting for RabbitMq process to exit...");

                var binPath = Path.Combine(workingPath, InstallationConstants.RabbitMq.BinDir);

                if (Directory.Exists(binPath))
                {
                    //rabbit mq uninstaller seems to be async so we need to monitor the install directory until it's empty
                    while (Directory.Exists(binPath) &&
                           Directory.EnumerateFiles(binPath).Any())
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();

                    //one last wait for system to release resources
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }

                CleanUpFolders(workingPath);
            }

            if (shouldDeleteService)
            {

                WriteVerbose("Removing RabbitMq windows service");

                try
                {
                    const string serviceToDelete = " delete RabbitMQ";
                    externalProcessRunner.Run("sc", Directory.GetCurrentDirectory(), serviceToDelete);
                }
                catch (Exception ex)
                {
                    WriteWarning("Failed to remove RabbitMq windows service. Clean removal might fail: " + ex.Message);
                }
            }


            WriteVerbose("Uninstallation process completed");
        }

        private void CleanUpFolders(string installPath)
        {
            var directoryCleaner = new DirectoryCleaner();

            try
            {
                directoryCleaner.Clean(installPath);
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