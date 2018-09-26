using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591
    public class Queue

    {
        public ThroughputDetails messages_details { get; set; }
        public long messages { get; set; }
        public ThroughputDetails messages_unacknowledged_details { get; set; }
        public long messages_unacknowledged { get; set; }
        public ThroughputDetails messages_ready_details { get; set; }
        public long messages_ready { get; set; }
        public ThroughputDetails reductions_details { get; set; }
        public long reductions { get; set; }
        public string node { get; set; }
        public QueueArguments arguments { get; set; }
        public bool exclusive { get; set; }
        public bool auto_delete { get; set; }
        public bool durable { get; set; }
        public string vhost { get; set; }
        public string name { get; set; }
        public long message_bytes_paged_out { get; set; }
        public long messages_paged_out { get; set; }
        public BackingQueueStatus backing_queue_status { get; set; }
        public int head_message_timestamp { get; set; }
        public long message_bytes_persistent { get; set; }
        public long message_bytes_ram { get; set; }
        public long message_bytes_unacknowledged { get; set; }
        public long message_bytes_ready { get; set; }
        public long message_bytes { get; set; }
        public long messages_persistent { get; set; }
        public long messages_unacknowledged_ram { get; set; }
        public long messages_ready_ram { get; set; }
        public long messages_ram { get; set; }
        public QueueGarbageCollection garbage_collection { get; set; }
        public string state { get; set; }
        public string[] recoverable_slaves { get; set; }
        public string[] synchronised_slave_nodes { get; set; }
        public string[] slave_nodes { get; set; }
        public int consumers { get; set; }
        public string exclusive_consumer_tag { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> effective_policy_definition { get; set; }
        public string operator_policy { get; set; }
        public string policy { get; set; }
        public decimal consumer_utilisation { get; set; }
        public string idle_since { get; set; }
        public long memory { get; set; }
    }
#pragma warning restore 1591
}
