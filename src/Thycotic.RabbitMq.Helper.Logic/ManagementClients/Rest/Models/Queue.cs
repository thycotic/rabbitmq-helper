using System.Collections.Generic;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591
    public class Queue

    {
        public QueueMessagesDetails messages_details { get; set; }
        public int messages { get; set; }
        public QueueMessagesDetails messages_unacknowledged_details { get; set; }
        public int messages_unacknowledged { get; set; }
        public QueueMessagesDetails messages_ready_details { get; set; }
        public int messages_ready { get; set; }
        public QueueMessagesDetails reductions_details { get; set; }
        public int reductions { get; set; }
        public string node { get; set; }
        public QueueArguments arguments { get; set; }
        public bool exclusive { get; set; }
        public bool auto_delete { get; set; }
        public bool durable { get; set; }
        public string vhost { get; set; }
        public string name { get; set; }
        public int message_bytes_paged_out { get; set; }
        public int messages_paged_out { get; set; }
        public BackingQueueStatus backing_queue_status { get; set; }
        public object head_message_timestamp { get; set; }
        public int message_bytes_persistent { get; set; }
        public int message_bytes_ram { get; set; }
        public int message_bytes_unacknowledged { get; set; }
        public int message_bytes_ready { get; set; }
        public int message_bytes { get; set; }
        public int messages_persistent { get; set; }
        public int messages_unacknowledged_ram { get; set; }
        public int messages_ready_ram { get; set; }
        public int messages_ram { get; set; }
        public QueueGarbageCollection garbage_collection { get; set; }
        public string state { get; set; }
        public object recoverable_slaves { get; set; }
        public int consumers { get; set; }
        public object exclusive_consumer_tag { get; set; }
        public List<object> effective_policy_definition { get; set; }
        public object operator_policy { get; set; }
        public object policy { get; set; }
        public object consumer_utilisation { get; set; }
        public string idle_since { get; set; }
        public int memory { get; set; }
    }
#pragma warning restore 1591
}
