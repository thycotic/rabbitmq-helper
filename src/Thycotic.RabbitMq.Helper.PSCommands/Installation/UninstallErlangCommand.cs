using System;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using Thycotic.CLI.Commands;
using Thycotic.Utility.IO;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    /// Uninstalls prior installation of Erlang
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://tempuri.org">TODO: Thycotic</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///   <para>TODO: This is part of the first example's introduction.</para>
    ///   <para>TODO: This is also part of the first example's introduction.</para>
    ///   <code>TODO: New-Thingy | Write-Host</code>
    ///   <para>TODO: This is part of the first example's remarks.</para>
    ///   <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsLifecycle.Uninstall, "Erlang")]
    public class UninstallErlangCommand : Cmdlet
    {
        /// <summary>
        /// Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {

            WriteVerbose("Uninstalling prior version of Erlang");

            var executablePath = InstallationConstants.Erlang.UninstallerPath;

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

            try
            {

                const string erlandProcessKill = " /F /IM epmd.exe";
                externalProcessRunner.Run("taskkill", workingPath, erlandProcessKill);
            }
            catch (Exception ex)
            {
                WriteWarning("Failed to terminate erlang process. Clean removal might fail: " + ex.Message);
            }

            WriteVerbose("Waiting for Erlang process to exit...");
            Task.Delay(TimeSpan.FromSeconds(15)).Wait();

            CleanUpFolders();

            WriteVerbose("Uninstallation process completed");

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
                WriteWarning("Failed to clean installation path. Clean removal might fail: " + ex.Message);
            }
        }
    }
}