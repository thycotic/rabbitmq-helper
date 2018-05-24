using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Creates a RabbitMq configuration directory
    /// </summary>
    /// <para type="synopsis">Creates a RabbitMq configuration directory</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>New-RabbitMqConfigDirectory</code>
    /// </example>
    [Cmdlet(VerbsCommon.New, "RabbitMqConfigDirectory")]
    public class NewRabbitMqConfigDirectoryCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (Directory.Exists(InstallationConstants.RabbitMq.ConfigurationPath))
            {
                WriteVerbose("RabbitMq configuration folder already exists");
                return;
            }

            WriteVerbose("Creating RabbitMq configuration folder");

            Directory.CreateDirectory(InstallationConstants.RabbitMq.ConfigurationPath);
        }
    }
}