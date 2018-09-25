using System;
using System.Management.Automation;
using System.Security.Authentication;
using RabbitMQ.Client;
using Thycotic.RabbitMq.Helper.Logic;

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
    public class AssertRabbitMqConnectivityCommand : Cmdlet
    {
        private static class ParameterSets
        {
            public const string Tls = "Tls";
        }

        /// <summary>
        ///     Gets or sets the use TLS.
        /// </summary>
        /// <value>
        ///     The use TLS.
        /// </value>
        /// <para type="description">Gets or sets the use TLS.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Tls)]
        public SwitchParameter UseTls { get; set; }

        /// <summary>
        ///     Gets or sets the hostname.
        /// </summary>
        /// <value>
        ///     The hostname.
        /// </value>
        /// <para type="description">Gets or sets the hostname.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Tls)]
        [Alias("SubjectName", "FQDN")]
        public string Hostname { get; set; } = DnsEx.GetDnsHostName();

        /// <summary>
        ///     Gets or sets the credential of the rabbit mq user.
        /// </summary>
        /// <value>
        ///     The credential of the rabbit mq user.
        /// </value>
        /// <para type="description">Gets or sets the credential of the rabbit mq user.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public PSCredential Credential { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteVerbose("Checking credentials");

            try
            {
                using (var connection = this.GetConnection(Hostname, Credential.UserName, Credential.GetNetworkCredential().Password, UseTls))
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
               throw new Exception("Connection failed. There might be an issue with the installation. Please check the RabbitMq log files:" + ex.Message, ex);
            }
        }

    }
}