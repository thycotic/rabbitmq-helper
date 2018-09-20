using System;
using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering
{
    /// <summary>
    ///     Gets the current cluster name
    /// </summary>
    /// <para type="synopsis"> Gets the current cluster name</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Remove-AllQueues</code>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "RabbitMqClusterName")]
    public class SetRabbitMqClusterCookieCommand : RestManagementConsoleCmdlet
    {
        /// <summary>
        /// Gets or sets the content of the cookie.
        /// </summary>
        /// <value>
        /// The content of the cookie.
        /// </value>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string CookieContent { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(CookieContent))
            {
                throw new Exception("No cookie content specified");
            }

            WriteVerbose($"Setting system cookie contents {InstallationConstants.Erlang.CookieSystemPath}");

            File.WriteAllText(InstallationConstants.Erlang.CookieSystemPath, CookieContent);

            WriteVerbose($"Setting user profile cookie contents {InstallationConstants.Erlang.CookieUserProfilePath}");

            File.WriteAllText(InstallationConstants.Erlang.CookieUserProfilePath, CookieContent);

            var client = new RabbitMqBatCtlClient();

            WriteVerbose("Stopping RabbitMq");
            client.HardStop();

            WriteVerbose("Starting RabbitMq");
            client.SoftStart();

            WriteVerbose("Cookie set");
        }

    }
}