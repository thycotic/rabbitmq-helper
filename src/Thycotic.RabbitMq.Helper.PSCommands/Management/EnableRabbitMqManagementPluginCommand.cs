using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Enables the RabbitMq management plugin (https://www.rabbitmq.com/management.html)
    /// </summary>
    /// <para type="synopsis"> Enables the RabbitMq management plugin (https://www.rabbitmq.com/management.html)</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Enable-RabbitMqManagementPlugin</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Enable, "RabbitMqManagementPlugin")]
    public class EnableRabbitMqManagementPluginCommand : ManagementConsoleCmdlet
    {
        /// <summary>
        ///     Gets or sets whether to open console when ready.
        /// </summary>
        /// <value>
        ///     The agree rabbit mq license.
        /// </value>
        /// <para type="description">Gets or sets whether to open console when ready.</para>
        [Parameter(
             Position = 0,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public SwitchParameter OpenConsoleAfterInstall { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            //we have to use local host because guest account does not work under FQDN
            const string pluginUrl = "http://localhost:15672/";
            const string executable = "rabbitmq-plugins.bat";
            var pluginsExecutablePath = Path.Combine(InstallationConstants.RabbitMq.BinPath, executable);

            var externalProcessRunner = new ExternalProcessRunner
            {
                EstimatedProcessDuration = TimeSpan.FromSeconds(15)
            };

            const string parameters2 = "enable rabbitmq_management";

            WriteVerbose("Enabling management console");

            externalProcessRunner.Run(pluginsExecutablePath, WorkingPath, parameters2);

            if (OpenConsoleAfterInstall)
            {
                WriteVerbose(string.Format("Opening management console at {0}", pluginUrl));
                Process.Start(pluginUrl);
            }
        }
    }
}