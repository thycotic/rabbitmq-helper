using System;
using System.IO;
using System.Management.Automation;
using Thycotic.CLI.Commands;
using Thycotic.Utility.OS;
using Thycotic.Utility.Reflection;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    /// Installs Erlang
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
    [Cmdlet(VerbsLifecycle.Install, "Erlang")]
    public class InstallErlangCommand : Cmdlet
    {
        /// <summary>
        /// Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">No installer found</exception>
        protected override void ProcessRecord()
        {

            var executablePath = GetErlangInstallerCommand.ErlangInstallerPath;

            if (!File.Exists(executablePath))
            {
                throw new FileNotFoundException("No installer found");
            }

            var externalProcessRunner = new ExternalProcessRunner
            {
                EstimatedProcessDuration = TimeSpan.FromMinutes(10)
            };


            var assemblyEntryPointProvider = new AssemblyEntryPointProvider();

            var workingPath = assemblyEntryPointProvider.GetAssemblyDirectory(this.GetType());

            const string silent = "/S";

            WriteVerbose("Installing Erlang...");

            externalProcessRunner.Run(executablePath, workingPath, silent);

            WriteVerbose("Installation process completed");

        }
    }
}