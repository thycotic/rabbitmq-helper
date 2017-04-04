using System;
using System.Management.Automation;
using RabbitMQ.Client;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Validates connectivity to RabbitMq
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
    [Cmdlet(VerbsLifecycle.Assert, "RabbitMqConnectivity")]
    public class AssertConnectivityCommand : ManagementConsoleCmdlet
    {
        /// <summary>
        ///     Gets or sets the use SSL.
        /// </summary>
        /// <value>
        ///     The use SSL.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = "SSL")]
        public SwitchParameter UseSsl { get; set; }

        /// <summary>
        ///     Gets or sets the hostname.
        /// </summary>
        /// <value>
        ///     The hostname.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = "SSL")]
        [Alias("SubjectName", "FQDN")]
        public string Hostname { get; set; }

        /// <summary>
        ///     Gets or sets the name of the rabbit mq user.
        /// </summary>
        /// <value>
        ///     The name of the rabbit mq user.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("RabbitMqUserName")]
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets the rabbit mq password.
        /// </summary>
        /// <value>
        ///     The rabbit mq password.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("RabbitMqPw", "RabbitMqPassword")]
        public string Password { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                using (var connection = GetConnection(Hostname, UserName, Password, UseSsl))
                {
                    using (var model = connection.CreateModel())
                    {
                        if (model.IsOpen)
                            WriteVerbose("Connection successful");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteWarning(
                    "Connection failed. There might be an issues with the installation. Please check the RabbitMq log files:" +
                    ex.Message);
            }
        }

        private IConnection GetConnection(string hostname, string userName, string password, bool useSsl)
        {
            const int nonSslPort = 5672;
            const int sslPost = 5671;
            //using FQDN to avoid running into errors when under SSL
            var url = string.Format("amqp://{0}:{1}", hostname, useSsl ? sslPost : nonSslPort);

            WriteVerbose(string.Format("Getting connection for {0}", url));

            var connectionFactory = new ConnectionFactory
            {
                Uri = url,
                RequestedHeartbeat = 300,
                UserName = userName,
                Password = password
            };

            if (useSsl)
            {
                var uri = new Uri(url);

                connectionFactory.Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = uri.Host
                    //AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors,
                };
            }

            return connectionFactory.CreateConnection();
        }
    }
}