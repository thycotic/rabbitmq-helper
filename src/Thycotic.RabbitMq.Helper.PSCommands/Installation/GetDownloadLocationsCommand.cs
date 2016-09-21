using System.Collections.Generic;
using System.Management.Automation;
using Thycotic.CLI.Commands;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    /// Gets download locations for Erlang and RabbitMq (most helpful when needing to do offline installation)
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://tempuri.org">TODO: Thycotic</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///   <para>TODO: This is part of the first example's introduction.</para>
    ///   <para>TODO: This is also part of the first example's introduction.</para>
    ///   <code>TODO: New-Thingy | Write-Host</code>
    ///   <para>TODO: This is part of the first example's remarks.</para>
    ///   <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "DownloadLocations")]
    [Alias("pdl")]
    [OutputType(typeof(KeyValuePair<string, string>))]
    public class GetDownloadLocationsCommand : Cmdlet
    {
        /// <summary>
        /// Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteObject(new KeyValuePair<string, string>("Erlang:", InstallationConstants.Erlang.DownloadUrl));
            WriteObject(new KeyValuePair<string, string>("RabbitMq:", InstallationConstants.RabbitMq.DownloadUrl));

        }
    }
}
