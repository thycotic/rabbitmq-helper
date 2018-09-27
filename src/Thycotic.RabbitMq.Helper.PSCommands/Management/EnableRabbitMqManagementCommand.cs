using System;
using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Enables the RabbitMq management UI (https://www.rabbitmq.com/management.html)
    /// </summary>
    /// <para type="synopsis"> Enables the RabbitMq management IO (https://www.rabbitmq.com/management.html)</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Enable-RabbitMqManagement</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Enable, "RabbitMqManagement")]
    public class EnableRabbitMqManagementCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatPluginClient();

            WriteVerbose("Enabling management UI");

            var output = client.EnableManagementUi();

            WriteVerbose(output);
        }
    }
}