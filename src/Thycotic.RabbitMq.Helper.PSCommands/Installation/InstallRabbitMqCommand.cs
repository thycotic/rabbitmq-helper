using System;
using System.IO;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.OS;
using Thycotic.RabbitMq.Helper.Logic.Reflection;
using Thycotic.RabbitMq.Helper.PSCommands.Management;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Installs RabbitMq
    /// </summary>
    /// <para type="synopsis">Installs RabbitMq</para>
    /// <para type="description">The Install-RabbitMq cmdlet will attempt to load the installed from Path.Combine(Path.GetTempPath(), "rabbitMq.exe");</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Get-RabbitMqInstaller</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Install-RabbitMq</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Install, "RabbitMq")]
    public class InstallRabbitMqCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.ApplicationException">The RABBITMQ_BASE environmental variable is not set correctly</exception>
        /// <exception cref="System.IO.FileNotFoundException">No installer found</exception>
        protected override void ProcessRecord()
        {
            var rabbitMqBase = Environment.GetEnvironmentVariable("RABBITMQ_BASE");

            if (rabbitMqBase != InstallationConstants.RabbitMq.ConfigurationPath)
            {
                WriteWarning(string.Format("RABBITMQ_BASE is set to {0}", rabbitMqBase));
                throw new ApplicationException("The RABBITMQ_BASE environmental variable is not set correctly");
            }

            var executablePath = GetRabbitMqInstallerCommand.RabbitMqInstallerPath;

            if (!File.Exists(executablePath))
                throw new FileNotFoundException("No installer found");

            var externalProcessRunner = new ExternalProcessRunner
            {
                EstimatedProcessDuration = TimeSpan.FromMinutes(10)
            };


            var assemblyEntryPointProvider = new AssemblyEntryPointProvider();

            var workingPath = assemblyEntryPointProvider.GetAssemblyDirectory(GetType());

            const string silent = "/S";

            WriteVerbose("Installing RabbitMq...");

            externalProcessRunner.Run(executablePath, workingPath, silent);

            WriteVerbose("Installation process completed");
        }
    }
}