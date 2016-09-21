using System.IO;
using System.Management.Automation;
using Thycotic.CLI.Commands;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    /// Creates a RabbitMq configuration directory  
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
    [Cmdlet(VerbsCommon.New, "RabbitMqConfigDirectory")]
    public class NewRabbitMqConfigDirectoryCommand : Cmdlet
    {
        /// <summary>
        /// Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {

            if (Directory.Exists(InstallationConstants.RabbitMq.ConfigurationPath))
            {
                return;
            }

            WriteVerbose("Creating RabbitMq configuration folder");

            Directory.CreateDirectory(InstallationConstants.RabbitMq.ConfigurationPath);

        }
    }
}