using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Disables the RabbitMq management UI (https://www.rabbitmq.com/management.html)
    /// </summary>
    /// <para type="synopsis"> Disables the RabbitMq management UI (https://www.rabbitmq.com/management.html)</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Enable-RabbitMqManagement</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Enable-RabbitMqManagement</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Disable, "RabbitMqManagement")]
    public class DisableRabbitMqManagementCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatPluginClient();

            WriteVerbose("Disabling management UI");

            var output = client.DisableManagementUi();

            WriteVerbose(output);
        }
    }
}