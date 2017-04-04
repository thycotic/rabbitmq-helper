using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Selects the tail of the RabbitMq log
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
    [Cmdlet(VerbsCommon.Get, "RabbitMqLog")]
    [Alias("tailRabbitmqLog", "tailLog")]
    [OutputType(typeof(string))]
    public class GetRabbitMqLogCommand : ManagementConsoleCmdlet
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetRabbitMqLogCommand" /> class.
        /// </summary>
        public GetRabbitMqLogCommand()
        {
            Count = 50;
        }

        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
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
            var logFile = string.Format("rabbit@{0}.log", Environment.MachineName);
            var logPath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath, "log", logFile);

            WriteVerbose(string.Format("Printing tail for {0}", logPath));

            var lockedFileReader = new LockedFileReader(logPath);

            var lines = lockedFileReader.GetTailLines(Count);

            lines.ToList().ForEach(WriteObject);
        }
    }
}