using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Stops RabbitMq
    /// </summary>
    /// <para type="synopsis">Stops RabbitMq</para>
    /// <para type="description">The Stop-RabbitMq cmdlet stops RabbitMq.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Stop-RabbitMq</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Stop, "RabbitMq")]
    public class StopRabbitMqCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatCtlClient();

            WriteVerbose("Stopping RabbitMq");

            client.Stop();
        }
    }
}