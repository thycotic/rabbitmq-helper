using System.Management.Automation;
using Thycotic.CLI.Commands;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Uninstalls Erlang and RabbitMq
    /// </summary>
    /// <para type="synopsis">Uninstalls Erlang and RabbitMq</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Uninstall-Erlang</para>
    /// <para type="link">Uninstall-RabbitMq</para>
    /// <para type="link">Install-Connector</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Uninstall-Connector</code>
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
            this.RequireRunningWithElevated();

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