using System;
using System.Management.Automation;
using System.Threading;
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
    [Cmdlet(VerbsLifecycle.Assert, "RabbitIsRunning")]
    public class AssertRabbitIsRunningCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var ctlInteractor = new CtlRabbitMqProcessInteractor();

            WriteVerbose("Reading RabbitMq status");

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            var output = string.Empty;

            while (!output.Contains("uptime") && !cts.IsCancellationRequested)
            {

                var parameters2 = "status";

                try
                {
                    output = ctlInteractor.Invoke(parameters2, TimeSpan.FromSeconds(15));
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            if (!output.Contains("uptime"))
            {
                throw new ApplicationException("Failed to get RabbitMq uptime information. RabbitMq is probably not running");
            }
        }
    }
}