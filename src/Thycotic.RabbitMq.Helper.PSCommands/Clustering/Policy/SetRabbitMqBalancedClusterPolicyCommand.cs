using System;
using System.Collections.Generic;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering.Policy
{
    /// <summary>
    ///     Creates a balanced policy on the RabbitMq node
    /// </summary>
    /// <para type="synopsis">Creates a balanced policy on the RabbitMq node</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Set-RabbitMqBalancedOneMirrorManualSyncClusterPolicy</code>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "RabbitMqBalancedClusterPolicy")]
    public class SetRabbitMqBalancedClusterPolicyCommand : PolicyCommand
    {

        /// <summary>
        /// Since RabbitMQ 3.6.0, masters perform synchronization in batches. Batch can be configured via the ha-sync-batch-size queue argument. Earlier versions will synchronise 1 message at a time by default. By synchronising messages in batches, the synchronisation process can be sped up considerably.
        /// To choose the right value for ha-sync-batch-size you need to consider: average message size, network throughput between RabbitMQ nodes, net_ticktime value
        /// For example, if you set ha-sync-batch-size to 50000 messages, and each message in the queue is 1KB, then each synchronisation message between nodes will be ~49MB.You need to make sure that your network between queue mirrors can accomodate this kind of traffic.If the network takes longer than net_ticktime to send one batch of messages, then nodes in the cluster could think they are in the presence of a network partition.
        /// </summary>
        /// <value>
        /// The size of the synchronize batch. Defaults to 400 since worst case message size is 256KB for Thycotic which in turn can be a 100MB synchronisation message.
        /// </value>
        /// <para type="description">
        /// Since RabbitMQ 3.6.0, masters perform synchronization in batches. Batch can be configured via the ha-sync-batch-size queue argument. Earlier versions will synchronise 1 message at a time by default. By synchronising messages in batches, the synchronisation process can be sped up considerably.
        /// To choose the right value for ha-sync-batch-size you need to consider: average message size, network throughput between RabbitMQ nodes, net_ticktime value
        /// For example, if you set ha-sync-batch-size to 50000 messages, and each message in the queue is 1KB, then each synchronisation message between nodes will be ~49MB.You need to make sure that your network between queue mirrors can accomodate this kind of traffic.If the network takes longer than net_ticktime to send one batch of messages, then nodes in the cluster could think they are in the presence of a network partition.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateRange(1, 5000)]
        public int SyncBatchSize { get; set; } = 400;

        /// <summary>
        /// Number of queue replicas (master plus mirrors) in the cluster.
        ///  A count value of 1 means just the queue master, with no mirrors.If the node running the queue master becomes unavailable, the behavior depends on queue durability.
        /// A count value of 2 means 1 queue master and 1 queue mirror. If the node running the queue master becomes unavailable, the queue mirror will be automatically promoted to master.In conclusion: NumberOfQueueMirrors = NumberOfNodes - 1
        /// </summary>
        /// <value>
        /// The size of the synchronize batch. Defaults to 400 since worst case message size is 256KB for Thycotic which in turn can be a 100MB synchronization message.
        /// </value>
        /// <para type="description">
        /// Number of queue replicas (master plus mirrors) in the cluster.
        /// A count value of 1 means just the queue master, with no mirrors.If the node running the queue master becomes unavailable, the behavior depends on queue durability.
        /// A count value of 2 means 1 queue master and 1 queue mirror. If the node running the queue master becomes unavailable, the queue mirror will be automatically promoted to master.In conclusion: NumberOfQueueMirrors = NumberOfNodes - 1
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateRange(1, 10)]
        public int QueueReplicaCount { get; set; } = 2;

        /// <summary>
        /// A queue will automatically synchronize when a new mirror joins. It is worth reiterating that queue synchronization is a blocking operation. If queues are small, or you have a fast network between RabbitMQ nodes and the ha-sync-batch-size was optimized, this is a good choice.
        /// </summary>
        /// <value>
        /// The automatic synchronize mode.
        /// </value>
        /// <para type="description">
        /// A queue will automatically synchronize when a new mirror joins. It is worth reiterating that queue synchronization is a blocking operation. If queues are small, or you have a fast network between RabbitMQ nodes and the ha-sync-batch-size was optimized, this is a good choice.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public SwitchParameter AutomaticSyncMode { get; set; }


        /// <inheritdoc />
        protected override IDictionary<string, object> GetPolicyDefinition()
        {
            var policy = new Dictionary<string, object>
            {
                {PolicyOptions.ParameterKeys.HaSyncBatchSize, SyncBatchSize},
                {
                    PolicyOptions.ParameterKeys.HaSyncMode,
                    AutomaticSyncMode ? PolicyOptions.HaSyncModes.Automatic : PolicyOptions.HaSyncModes.Manual
                },
                {PolicyOptions.ParameterKeys.QueueMasterLocator, PolicyOptions.QueueMasterLocation.MinMasters}
            };
            
            WriteVerbose($"Using replica count of {QueueReplicaCount}");

            policy.Add(PolicyOptions.ParameterKeys.HaMode, PolicyOptions.HaModes.Exactly);
            policy.Add(PolicyOptions.ParameterKeys.HaParams, QueueReplicaCount);
            
            return policy;
        }
    }
}