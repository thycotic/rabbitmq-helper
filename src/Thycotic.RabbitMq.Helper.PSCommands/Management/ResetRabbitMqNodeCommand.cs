using System;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Returns a RabbitMQ node to its virgin state. Removes the node from any cluster it belongs to, removes all data from the management database, such as configured users and vhosts, and deletes all persistent messages.
    /// </summary>
    /// <para type="synopsis">Returns a RabbitMQ node to its virgin state. Removes the node from any cluster it belongs to, removes all data from the management database, such as configured users and vhosts, and deletes all persistent messages.</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Reset-RabbitMqNodeCommand</code>
    /// </example>
    [Cmdlet(VerbsCommon.Reset, "RabbitMqNodeCommand")]
    public class ResetRabbitMqNodeCommand : Cmdlet
    {
        /// <summary>
        ///     Gets or sets a value indicating whether to force reset and avoid prompting.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [force reset]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets a value indicating whether to force reset and avoid prompting.</para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("ForceReset")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {

            if (Force || ShouldContinue(
                    "Are you sure you want to reset this RabbitMq node? Removes the node from any cluster it belongs to, removes all data from the management database, such as configured users and vhosts, and deletes all persistent messages.",
                    "Confirm"))
            {
                WriteVerbose("Reset request confirmed");
            }
            else
            {
                WriteVerbose("Reset aborted");
                return;
            }

            WriteVerbose($"Resetting {Environment.MachineName}");

            var client = new RabbitMqBatCtlClient();

            WriteVerbose("Stopping RabbitMq application");
            client.SoftStop();

            try
            {
                WriteVerbose($"Resetting");
                client.Reset();
            }
            finally
            {
                WriteVerbose("Starting RabbitMq application");
                client.SoftStart();
            }

        }
    }
}