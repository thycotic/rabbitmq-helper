namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591
    public class MetricsGcQueueLength
    {
        public int connection_closed { get; set; }
        public int channel_closed { get; set; }
        public int consumer_deleted { get; set; }
        public int exchange_deleted { get; set; }
        public int queue_deleted { get; set; }
        public int vhost_deleted { get; set; }
        public int node_node_deleted { get; set; }
        public int channel_consumer_deleted { get; set; }
    }
#pragma warning restore 1591
}