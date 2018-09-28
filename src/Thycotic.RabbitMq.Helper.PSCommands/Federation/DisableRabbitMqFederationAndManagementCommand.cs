using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Federation
{
    /// <summary>
    ///     Disables the RabbitMq federation and federation management UI
    /// </summary>
    /// <para type="synopsis">Disables the RabbitMq federation and federation management UI</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Enable-RabbitMqManagement</para>
    /// <para type="link">Enable-RabbitMqFederationAndManagement</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Disable-RabbitMqFederationAndManagement</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Disable, "RabbitMqFederationAndManagement")]
    public class DisableRabbitMqFederationAndManagementCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatPluginClient();

            WriteVerbose("Disabling federation and federation management UI");

            var output = client.DisableFederationAndManagementUi();

            WriteVerbose(output);
        }
    }
}