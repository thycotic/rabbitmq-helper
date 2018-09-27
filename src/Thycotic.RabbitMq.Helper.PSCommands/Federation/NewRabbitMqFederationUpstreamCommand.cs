using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Sockets;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.PSCommands.Federation
{
    /// <summary>
    ///     Creates a federation upstream on the RabbitMq node. This version does NOT support TLS connections
    /// </summary>
    /// <para type="synopsis">Creates a federation upstream on the RabbitMq node. This version does NOT support TLS connections</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Set-RabbitMqFederationUpstream</code>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "RabbitMqFederationUpstream")]
    public class SetRabbitMqFederationUpstreamCommand : RestManagementConsoleCmdlet
    {

        /// <summary>
        ///     Gets or sets the name of the upstream.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        /// <para type="description">Gets or sets the name.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the hostname of the upstream.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        /// <para type="description">Gets or sets the hostname.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("SubjectName", "FQDN")]
        public string Hostname { get; set; }

        /// <summary>
        /// Port to connect to the upstream on.
        /// </summary>
        /// <value>
        /// The port connect to the upstream on.
        /// </value>
        /// <para type="description">
        /// Port to connect to the upstream on.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateRange(1, 65535)]
        public int Port { get; set; } = 5672;

        /// <summary>
        ///     Gets or sets the credential of the rabbit mq user to connect with on the upstream.
        /// </summary>
        /// <value>
        ///     The credential of the rabbit mq user to connect with on the upstream.
        /// </value>
        /// <para type="description"> Gets or sets the credential of the rabbit mq user to connect with on the upstream.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public PSCredential Credential { get; set; }

        /// <summary>
        ///     Gets or sets the virtual host.
        /// </summary>
        /// <value>
        ///     The virtual host.
        /// </value>
        /// <para type="description">Gets or sets the virtual host.</para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string VirtualHost { get; set; } = Constants.RabbitMq.DefaultVirtualHost;

        /// <summary>
        /// The upstream will be defined to buffer messages when disconnected for up to one the specified number of milliseconds
        /// </summary>
        /// <value>
        /// The size of the synchronize batch. Defaults to 400 since worst case message size is 256KB for Thycotic which in turn can be a 100MB synchronisation message.
        /// </value>
        /// <para type="description">
        /// The upstream will be defined to buffer messages when disconnected for up to one the specified number of milliseconds
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateRange(600000, 3600000)]
        public int Expires { get; set; } = 3600000;

        /// <summary>
        /// Pre-fetch count.
        /// </summary>
        /// <value>
        /// The pre-fetch count.
        /// </value>
        /// <para type="description">
        /// The pre-fetch count.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateRange(100, 10000)]
        public int PreFetchCount { get; set; } = 1000;

        /// <summary>
        /// Reconnect delay in seconds.
        /// </summary>
        /// <value>
        /// The reconnect delay in seconds.
        /// </value>
        /// <para type="description">
        /// The reconnect delay in seconds.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateRange(1, 300)]
        public int ReconnectDelay { get; set; } = 5;

        /// <summary>
        /// Gets or sets that the firewall configured.
        /// </summary>
        /// <value>
        /// The firewall configured.
        /// </value>
        /// <para type="description">
        /// Gets or sets that the firewall configured. 
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public SwitchParameter FirewallConfigured { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqRestClient(BaseUrl, AdminCredential.UserName,
                AdminCredential.GetNetworkCredential().Password);

            int[] allPorts = { Port };
            int[] livePorts = { Port };

            if (FirewallConfigured || ShouldContinue(
                    $"Do you have private/domain firewall ports {string.Join(", ", allPorts)} open on this and the other RabbitMq node?",
                    "Firewall"))
            {
                WriteVerbose("Firewall/network access marked as configured");
            }
            else
            {
                throw new ApplicationException("Network access not configured");
            }

            WriteVerbose("Checking ports");

            using (var tcpClient = new TcpClient())
            {
                livePorts.ToList().ForEach(p =>
                {
                    try
                    {
                        WriteVerbose($"Validating connection to {Hostname} on port {p}");

                        tcpClient.Connect(Hostname, p);

                        tcpClient.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to connect to {Hostname} on port {p}", ex);
                    }
                });
            }

            WriteVerbose("Checking credentials");

            try
            {
                using (var connection = this.GetConnection(Hostname, Credential.UserName, Credential.GetNetworkCredential().Password, false, Port))
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

            WriteVerbose($"Federating {Environment.MachineName} to {Hostname} upstream ");
            
            var uriBuild = new UriBuilder
            {
                Scheme = "amqp",
                Host = Hostname,
                Port = Port,
                UserName = Uri.EscapeDataString(Credential.UserName),
                Password = Credential.GetNetworkCredential().Password
            };

            //get rid of trailing slashes
            var uriString = uriBuild.Uri.ToString().TrimEnd('/');

            var parameters = new ComponentParameters
            {
                component = ComponentParameterOptions.Federation.Component,
                name =  Name,
                value = new Dictionary<string, object>
            {
                {ComponentParameterOptions.Federation.ParameterKeys.AckMode, ComponentParameterOptions.Federation.AckModes.OnConfirm},

                    { ComponentParameterOptions.Federation.ParameterKeys.PreFetchCount, PreFetchCount},
                    { ComponentParameterOptions.Federation.ParameterKeys.ReconnectDelay, ReconnectDelay},
                    { ComponentParameterOptions.Federation.ParameterKeys.Expires, Expires},
                    { ComponentParameterOptions.Federation.ParameterKeys.TrustUserId, false},
                    { ComponentParameterOptions.Federation.ParameterKeys.Uri, uriString }
            }};
            
            client.CreateFederationUpstream(VirtualHost, Name, parameters);


            //remove the uri since it has a username and password
            parameters.value[ComponentParameterOptions.Federation.ParameterKeys.Uri] = "[redacted]";

            WriteVerbose("Policy settings:");
            parameters.value.OrderBy(pd => pd.Key).ToList().ForEach(dv => WriteVerbose($"{dv.Key} = {dv.Value}"));


            WriteVerbose("Upstream created/updated");
        }
    }
}