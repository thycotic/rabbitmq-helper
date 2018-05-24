using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Selects the tail of the RabbitMq Simple Authentication and Security Layer (SASL) log
    /// </summary>
    /// <para type="synopsis">Selects the tail of the RabbitMq Simple Authentication and Security Layer (SASL) log</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Get-RabbitMqLog</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Get-RabbitMqSaslLog</code>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "RabbitMqSaslLog")]
    [Alias("tailRabbitmqSqslLog", "tailsasllog")]
    [OutputType(typeof(string))]
    public class GetRabbitMqSaslLogCommand : Cmdlet
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetRabbitMqSaslLogCommand" /> class.
        /// </summary>
        public GetRabbitMqSaslLogCommand()
        {
            Count = 50;
        }

        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        /// <para type="description">Gets or sets the count.</para>
        [Parameter(
             Position = 0,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public int Count { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var logFile = string.Format("rabbit@{0}-sasl.log", Environment.MachineName);
            var logPath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath, "log", logFile);

            WriteVerbose(string.Format("Printing tail for {0}", logPath));

            var lockedFileReader = new LockedFileReader(logPath);

            var lines = lockedFileReader.GetTailLines(Count);

            lines.ToList().ForEach(WriteObject);
        }
    }
}