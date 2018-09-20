using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Sockets;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.PSCommands.Management;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering
{
    /// <summary>
    ///     Joins a RabbitMq cluster
    /// </summary>
    /// <para type="synopsis"> Joins a RabbitMq cluster</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Join-RabbitMqCluster</code>
    /// </example>
    [Cmdlet(VerbsCommon.Join, "RabbitMqCluster")]
    public class JoinRabbitMqClusterCommand : Cmdlet
    {
        /// <summary>
        /// Gets or sets name of the other node.
        /// </summary>
        /// <value>
        /// The name of the other node.
        /// </value>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string OtherNodeName { get; set; }
        
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
        /// Gets or sets that the cookie is configured.
        /// </summary>
        /// <value>
        /// The cookie set.
        /// </value>
        /// <para type="description">
        /// Gets or sets that the cookie is configured.
        /// the license.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public SwitchParameter CookieSet { get; set; }


        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            int[] allPorts = { 4369, 25672, 44002 };
            int[] livePorts = { 44002 };

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

            using (var tcpClient = new TcpClient())
            {
                livePorts.ToList().ForEach(p =>
                {
                    try
                    {
                        WriteVerbose($"Validating connection to {OtherNodeName} on port {p}");

                        tcpClient.Connect(OtherNodeName, p);

                        tcpClient.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to connect to {OtherNodeName} on port {p}", ex);
                    }
                });
            }

            if (CookieSet || ShouldContinue(
                "Have you set the Erlang cookie to match the one on the node your are joining? See Set-ErlangCookieFileCommand for details",
                "Cluster cookie"))
            {
                WriteVerbose("Erlang cookie set the same between this and target node");
            }
            else
            {
                throw new ApplicationException("Erlang cookie not set for cluster");
            }

            WriteVerbose($"Clustering {Environment.MachineName} to {OtherNodeName}");

            var client = new RabbitMqBatCtlClient();

            WriteVerbose("Stopping RabbitMq application");
            client.SoftStop();
            
            WriteVerbose($"Joining {OtherNodeName}");
            client.JoinCluster(OtherNodeName);
            
            WriteVerbose("Starting RabbitMq application");
            client.SoftStart();

        }
    }
}