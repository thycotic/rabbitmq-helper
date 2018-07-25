using System;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using Microsoft.Win32;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.IO;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Uninstalls prior installation of Erlang
    /// </summary>
    /// <para type="synopsis">Uninstalls prior installation of Erlang</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Uninstall-RabbitMq</para>
    /// <para type="link">Install-Connector</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Uninstall-Erlang</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Uninstall, "Erlang")]
    public class UninstallErlangCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteVerbose("Uninstalling prior versions of Erlang");

            var executablePaths = InstallationConstants.Erlang.UninstallerPaths;

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

                var externalProcessRunner = new ExternalProcessRunner();

                const string silent = "/S";

                externalProcessRunner.Run(executablePath, workingPath, silent);
                
                try
                {
                    const string erlandProcessKill = " /F /IM erl.exe";
                    externalProcessRunner.Run("taskkill", workingPath, erlandProcessKill);
                }
                catch (Exception ex)
                {
                    WriteWarning("Failed to terminate erl process. Clean removal might fail: " + ex.Message);
                }

                try
                {
                    const string erlandProcessKill = " /F /IM erlsrv.exe";
                    externalProcessRunner.Run("taskkill", workingPath, erlandProcessKill);
                }
                catch (Exception ex)
                {
                    WriteWarning("Failed to terminate erlsrv process. Clean removal might fail: " + ex.Message);
                }

                try
                {
                    const string erlandProcessKill = " /F /IM epmd.exe";
                    externalProcessRunner.Run("taskkill", workingPath, erlandProcessKill);
                }
                catch (Exception ex)
                {
                    WriteWarning("Failed to terminate epmd process. Clean removal might fail: " + ex.Message);
                }

                WriteVerbose("Waiting for Erlang process to exit...");
                Task.Delay(TimeSpan.FromSeconds(15)).Wait();

                CleanUpFolders(workingPath);
            }

            WriteVerbose("Removing Erlang registry information");

            var keyName = @"SOFTWARE\Ericsson";
            using (var rootKey = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                if (rootKey == null)
                {
                    WriteVerbose("Root key not found");
                }
                else
                {
                    using (var erlangKey = rootKey.OpenSubKey(keyName, true))
                    {
                        if (erlangKey == null)
                        {
                            WriteVerbose("Erlang key not found");
                            return;
                        }
                    }

                    rootKey.DeleteSubKeyTree("Erlang");

                    WriteVerbose("Key removed");
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
        }
    }
}