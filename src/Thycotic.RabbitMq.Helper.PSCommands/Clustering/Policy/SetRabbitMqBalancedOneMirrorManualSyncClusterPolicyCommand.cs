using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;
using Thycotic.RabbitMq.Helper.PSCommands.Clustering.Policy;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering
{
    /// <summary>
    ///     Creates a policy on the RabbitMq node
    /// </summary>
    /// <para type="synopsis">Deletes all queues in the current instance of RabbitMq</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Set-RabbitMqBalancedOneMirrorManualSyncClusterPolicy</code>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "RabbitMqBalancedOneMirrorManualSyncClusterPolicy")]
    public class SetRabbitMqBalancedOneMirrorManualSyncClusterPolicyCommand : ClusterPolicyCommand
    {
        
        /// <summary>
        /// Since RabbitMQ 3.6.0, masters perform synchronisation in batches. Batch can be configured via the ha-sync-batch-size queue argument. Earlier versions will synchronise 1 message at a time by default. By synchronising messages in batches, the synchronisation process can be sped up considerably.
        /// To choose the right value for ha-sync-batch-size you need to consider: average message size, network throughput between RabbitMQ nodes, net_ticktime value
        /// For example, if you set ha-sync-batch-size to 50000 messages, and each message in the queue is 1KB, then each synchronisation message between nodes will be ~49MB.You need to make sure that your network between queue mirrors can accomodate this kind of traffic.If the network takes longer than net_ticktime to send one batch of messages, then nodes in the cluster could think they are in the presence of a network partition.
        /// </summary>
        /// <value>
        /// The size of the synchronize batch.
        /// </value>
        /// <para type="description">
        /// Since RabbitMQ 3.6.0, masters perform synchronisation in batches. Batch can be configured via the ha-sync-batch-size queue argument. Earlier versions will synchronise 1 message at a time by default. By synchronising messages in batches, the synchronisation process can be sped up considerably.
        /// To choose the right value for ha-sync-batch-size you need to consider: average message size, network throughput between RabbitMQ nodes, net_ticktime value
        /// For example, if you set ha-sync-batch-size to 50000 messages, and each message in the queue is 1KB, then each synchronisation message between nodes will be ~49MB.You need to make sure that your network between queue mirrors can accomodate this kind of traffic.If the network takes longer than net_ticktime to send one batch of messages, then nodes in the cluster could think they are in the presence of a network partition.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string SyncBatchSize { get; set; }

        /// <inheritdoc />
        protected override Logic.ManagementClients.Rest.Models.Policy GetPolicy()
        {
            int syncBatchSizeInt;
            if (string.IsNullOrWhiteSpace(SyncBatchSize))
            {
                syncBatchSizeInt = 5000;
            }
            else
            {
                int.TryParse(SyncBatchSize, out syncBatchSizeInt);
            }

            if (syncBatchSizeInt < 0)
            {
                syncBatchSizeInt = 5000;
            }

            return new Logic.ManagementClients.Rest.Models.Policy
            {
                pattern = Pattern,
                definition = new PolicyDefinition
                {
                    ha_mode = PolicyOptions.HaModes.Exactly,
                    ha_params = 2.ToString(), //1 master + 1 mirror
                    ha_sync_batch_size = syncBatchSizeInt.ToString(),
                    ha_sync_mode = PolicyOptions.HaSyncModes.Manual,
                    queue_master_locator = PolicyOptions.QueueMasterLocation.MinMasters
                }
                //TODO: Apply apply-to and priority
            };
        }
    }
}