using System.Collections.Generic;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Gets download locations for Erlang and RabbitMq (most helpful when needing to do offline installation)
    /// </summary>
    /// <para type="synopsis">Gets download locations for Erlang and RabbitMq (most helpful when needing to do offline installation)</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Get-ErlangInstaller</para>
    /// <para type="link">Get-RabbitMqInstaller</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Get-DownloadLocations</code>
    /// </example>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Get-DownloadLocations -UseThycoticMirror</code>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "DownloadLocations")]
    [Alias("pdl")]
    [OutputType(typeof(KeyValuePair<string, string>))]
    public class GetDownloadLocationsCommand : Cmdlet
    {

        /// <summary>
        ///     Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.
        /// </summary>
        /// <value>
        ///     <c>true</c> if mirror will be used; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("Mirror")]
        public SwitchParameter UseThycoticMirror { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteObject(new KeyValuePair<string, string>("Erlang:",
                UseThycoticMirror
                    ? InstallationConstants.Erlang.ThycoticMirrorDownloadUrl
                    : InstallationConstants.Erlang.DownloadUrl));

            WriteObject(new KeyValuePair<string, string>("RabbitMq:",
                UseThycoticMirror
                    ? InstallationConstants.RabbitMq.ThycoticMirrorDownloadUrl
                    : InstallationConstants.RabbitMq.DownloadUrl));
        }
    }
}