using System.Management.Automation;
using Thycotic.CLI.Commands;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Uninstalls Erlang and RabbitMq
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///     <para>TODO: This is part of the first example's introduction.</para>
    ///     <para>TODO: This is also part of the first example's introduction.</para>
    ///     <code>TODO: New-Thingy | Write-Host</code>
    ///     <para>TODO: This is part of the first example's remarks.</para>
    ///     <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsLifecycle.Uninstall, "Connector")]
    [Alias("uninstallConnector")]
    public class UninstallConnectorCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            const int activityid = 7;
            const string activity = "Uninstalling";

            WriteProgress(new ProgressRecord(activityid, activity, "Uninstalling RabbitMq") {PercentComplete = 10});

            new UninstallRabbitMqCommand().AsChildOf(this).InvokeImmediate();

            WriteProgress(new ProgressRecord(activityid, activity, "Uninstalling Erlang") {PercentComplete = 30});

            new UninstallErlangCommand().AsChildOf(this).InvokeImmediate();

            WriteProgress(new ProgressRecord(activityid, activity, "Done") {RecordType = ProgressRecordType.Completed});


            WriteVerbose("Connector has been uninstalled.");
        }
    }
}