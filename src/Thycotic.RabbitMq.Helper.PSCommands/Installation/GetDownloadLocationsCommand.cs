using System.Collections.Generic;
using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Gets download locations for Erlang and RabbitMq (most helpful when needing to do offline installation)
    /// </summary>
    /// <para type="synopsis">Gets download locations for Erlang and RabbitMq (most helpful when needing to do offline installation)</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Get-ErlangInstaller</para>
    /// <para type="link">Get-RabbitMqInstaller</para>
    /// <example>
    ///     <para>TODO: This is part of the first example's introduction.</para>
    ///     <para>TODO: This is also part of the first example's introduction.</para>
    ///     <code>TODO: New-Thingy | Write-Host</code>
    ///     <para>TODO: This is part of the first example's remarks.</para>
    ///     <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "DownloadLocations")]
    [Alias("pdl")]
    [OutputType(typeof(KeyValuePair<string, string>))]
    public class GetDownloadLocationsCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteObject(new KeyValuePair<string, string>("Erlang:", InstallationConstants.Erlang.DownloadUrl));
            WriteObject(new KeyValuePair<string, string>("RabbitMq:", InstallationConstants.RabbitMq.DownloadUrl));
        }
    }
}