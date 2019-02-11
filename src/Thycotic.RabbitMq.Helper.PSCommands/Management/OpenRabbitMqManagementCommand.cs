using System;
using System.Diagnostics;
using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///    Opens the RabbitMq management plugin (https://www.rabbitmq.com/management.html)
    /// </summary>
    /// <para type="synopsis"> Opens the RabbitMq management plugin (https://www.rabbitmq.com/management.html)</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Enable-RabbitMqManagementPlugin</code>
    /// </example>
    [Cmdlet(VerbsCommon.Open, "RabbitMqManagement")]
    public class OpenRabbitMqManagementCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            //we have to use local host because guest account does not work under FQDN
            const string pluginUrl = "http://localhost:15672/";

            WriteVerbose($"Opening management console at {pluginUrl}");

            try
            {
                Process.Start(pluginUrl);
            }
            catch (Exception ex)
            {
                WriteVerbose($"Failed to open console: {ex.Message}. This is not an error if running non-interactive. Visit {pluginUrl} manually.");

            }
        }
    }
}