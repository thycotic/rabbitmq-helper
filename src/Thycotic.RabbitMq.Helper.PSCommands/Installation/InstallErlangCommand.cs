using System;
using System.IO;
using System.Management.Automation;
using Thycotic.Utility.OS;
using Thycotic.Utility.Reflection;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Installs Erlang
    /// </summary>
    /// <para type="synopsis">Installs Erlang</para>
    /// <para type="description">Tee Install-Erlang cmdlet will attempt to load the installed from Path.Combine(Path.GetTempPath(), "erlang.exe");</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Get-ErlangInstaller</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Install-Erlang</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Install, "Erlang")]
    public class InstallErlangCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">No installer found</exception>
        protected override void ProcessRecord()
        {
            var executablePath = GetErlangInstallerCommand.ErlangInstallerPath;

            if (!File.Exists(executablePath))
                throw new FileNotFoundException("No installer found");

            var externalProcessRunner = new ExternalProcessRunner
            {
                EstimatedProcessDuration = TimeSpan.FromMinutes(10)
            };


            var assemblyEntryPointProvider = new AssemblyEntryPointProvider();

            var workingPath = assemblyEntryPointProvider.GetAssemblyDirectory(GetType());

            const string silent = "/S";

            WriteVerbose("Installing Erlang...");

            externalProcessRunner.Run(executablePath, workingPath, silent);

            WriteVerbose("Installation process completed");
        }
    }
}