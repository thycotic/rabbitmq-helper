using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Starts RabbitMq
    /// </summary>
    /// <para type="synopsis">Starts RabbitMq</para>
    /// <para type="description">The Stop-RabbitMq cmdlet starts RabbitMq.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Start-RabbitMq</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Start, "RabbitMq")]
    public class StartRabbitMqCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatCtlClient();

            WriteVerbose("Starting RabbitMq");

            client.SoftStart();
        }
    }
}