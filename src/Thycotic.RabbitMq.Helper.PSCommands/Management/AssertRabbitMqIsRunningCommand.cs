using System;
using System.Management.Automation;
using System.Threading;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Validates RabbitMq is running on the current host.
    /// </summary>
    /// <para type="synopsis">Validates RabbitMq is running on the current host.</para>
    /// <para type="description">The Assert-RabbitIsRunning attempts read the status of RabbitMq.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Assert-RabbitIsRunning</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Assert, "RabbitMqIsRunning")]
    public class AssertRabbitMqIsRunningCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatCtlClient();

            WriteVerbose("Reading RabbitMq status");

            if (!client.IsRunning)
            {
                throw new Exception("RabbitMq is not running");
            }

            WriteVerbose("RabbitMq is running");
        }
    }
}