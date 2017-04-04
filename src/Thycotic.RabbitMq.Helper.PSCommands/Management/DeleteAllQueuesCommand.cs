using System;
using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Enables the RabbitMq management plugin (https://www.rabbitmq.com/management.html)
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://tempuri.org">TODO: Thycotic</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///     <para>TODO: This is part of the first example's introduction.</para>
    ///     <para>TODO: This is also part of the first example's introduction.</para>
    ///     <code>TODO: New-Thingy | Write-Host</code>
    ///     <para>TODO: This is part of the first example's remarks.</para>
    ///     <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsLifecycle.Enable, "DeleteAllQueues")]
    public class DeleteAllQueuesCommand : ManagementConsoleCmdlet
    {
        /// <summary>
        ///     Gets or sets the agree rabbit mq license.
        /// </summary>
        /// <value>
        ///     The agree rabbit mq license.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             Position = 0,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public SwitchParameter OpenConsoleAfterInstall { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            throw new NotImplementedException();

//            $cred = Get - Credential
//$result = iwr - ContentType 'application/json' - Method Get - Credential $cred   'http://localhost:15672/api/queues' | % {
//                ConvertFrom - Json  $_.Content } | % { $_ } | ? { $_.messages - eq 0} | % {
//                iwr - method DELETE - Credential $cred - uri  $("http://localhost:15672/api/queues/{0}/{1}" - f[System.Web.HttpUtility]::UrlEncode($_.vhost),  $_.name)
// }

//            Write - Host 'Empty queues were deleted'


            ////we have to use local host because guest account does not work under FQDN
            //const string pluginUrl = "http://localhost:15672/";
            //const string executable = "rabbitmq-plugins.bat";
            //var pluginsExecutablePath = Path.Combine(InstallationConstants.RabbitMq.BinPath, executable);

            //var externalProcessRunner = new ExternalProcessRunner
            //{
            //    EstimatedProcessDuration = TimeSpan.FromSeconds(15)
            //};

            //const string parameters2 = "enable rabbitmq_management";

            //externalProcessRunner.Run(pluginsExecutablePath, WorkingPath, parameters2);

            //if (OpenConsoleAfterInstall)
            //{
            //    WriteVerbose(string.Format("Opening management console at {0}", pluginUrl));
            //    Process.Start(pluginUrl);
            //}
        }
    }
}