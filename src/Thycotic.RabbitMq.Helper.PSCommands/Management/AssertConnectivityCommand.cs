using System;
using System.Management.Automation;
using RabbitMQ.Client;
using Thycotic.Utility;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Validates connectivity to RabbitMq
    /// </summary>
    /// <para type="synopsis">Validates connectivity to RabbitMq</para>
    /// <para type="description">The Assert-RabbitMqConnectivity attempts to connect to RabbitMq the same way a Distributed Engine or Secret Server would.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Assert-RabbitMqConnectivity</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Assert, "RabbitMqConnectivity")]
    public class AssertConnectivityCommand : Cmdlet
    {
        /// <summary>
        ///     Gets or sets the use SSL.
        /// </summary>
        /// <value>
        ///     The use SSL.
        /// </value>
        /// <para type="description">Gets or sets the use SSL.</para>
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
        /// <para type="description">Gets or sets the hostname.</para>
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
        /// <para type="description">Gets or sets the name of the rabbit mq user.</para>
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
        /// <para type="description">Gets or sets the rabbit mq password.</para>
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
            Hostname = Hostname ?? DnsEx.GetDnsHostName();

            try
            {
                using (var connection = GetConnection(Hostname, UserName, Password, UseSsl))
                {
                    using (var model = connection.CreateModel())
                    {
                        if (model.IsOpen)
                        {
                            WriteVerbose("Connection successful");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteWarning(
                    "Connection failed. There might be an issue with the installation. Please check the RabbitMq log files:" +
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
                Uri = new Uri(url),
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