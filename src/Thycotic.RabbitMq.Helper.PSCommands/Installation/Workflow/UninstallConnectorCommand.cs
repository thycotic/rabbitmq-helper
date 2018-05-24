using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.Workflow;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation.Workflow
{
    /// <summary>
    ///     Uninstalls Erlang and RabbitMq
    /// </summary>
    /// <para type="synopsis">Uninstalls Erlang and RabbitMq</para>
    /// <para type="description"></para>
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
            using (var workflow = new CmdletWorkflow(this, "Un-installing"))
            {
                workflow
                    .ReportProgress("Un-installing RabbitMq", 10)
                    .Then(() => new UninstallRabbitMqCommand())

                    .ReportProgress("Un-installing Erlang", 30)
                    .Then(() => new UninstallErlangCommand())

                    .Invoke();

                WriteVerbose("Connector has been un-installed.");
            }
        }
    }
}