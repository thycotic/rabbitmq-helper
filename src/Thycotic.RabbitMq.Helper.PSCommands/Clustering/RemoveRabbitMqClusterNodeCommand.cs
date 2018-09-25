using System;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering
{
    /// <summary>
    ///     Removes a node from the current nodes cluster. Use when a node is not responsive and/or cannot leave the cluster 
    /// </summary>
    /// <para type="synopsis">Removes a node from the current nodes cluster. Use when a node is not responsive and/or cannot leave the cluster </para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Reset-RabbitMqNodeCommand</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Remove-RabbitMqClusterNode UnresponsiveNode1</code>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "RabbitMqClusterNode")]
    public class RemoveRabbitMqClusterNodeCommand : Cmdlet
    {
        /// <summary>
        /// Gets or sets name of the other node.
        /// </summary>
        /// <value>
        /// The name of the other node.
        /// </value>
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string OtherNodeName { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteVerbose($"Removing {OtherNodeName} from cluster");

            var client = new RabbitMqBatCtlClient();

            WriteVerbose($"Removing {OtherNodeName}");
            client.RemoveFromClusterCluster(OtherNodeName);
        }
    }
}