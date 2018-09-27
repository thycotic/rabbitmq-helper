using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Federation
{
    /// <summary>
    ///     Enables the RabbitMq federation and federation management UI
    /// </summary>
    /// <para type="synopsis">Enables the RabbitMq federation and federation management UI</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Enable-RabbitMqManagement</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Enable-RabbitMqFederationAndManagement</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Enable, "RabbitMqFederationAndManagement")]
    public class EnableRabbitMqFederationAndManagementCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatPluginClient();

            WriteVerbose("Enabling federation and management UI");

            var output = client.EnableFederationAndManagementUi();

            WriteVerbose(output);
        }
    }
}