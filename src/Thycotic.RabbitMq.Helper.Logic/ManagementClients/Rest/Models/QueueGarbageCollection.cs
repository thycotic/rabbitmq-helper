namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591
    public class QueueGarbageCollection
    {
        public int minor_gcs { get; set; }
        public int fullsweep_after { get; set; }
        public int min_heap_size { get; set; }
        public int min_bin_vheap_size { get; set; }
        public int max_heap_size { get; set; }
    }
#pragma warning restore 1591
}