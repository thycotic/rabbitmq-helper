using System.Collections.Generic;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591
    public class Node
    {
        //TODO: Figure out the schema
        public List<object> partitions { get; set; }
        public string os_pid { get; set; }
        public int fd_total { get; set; }
        public int sockets_total { get; set; }
        public long mem_limit { get; set; }
        public bool mem_alarm { get; set; }
        public int disk_free_limit { get; set; }
        public bool disk_free_alarm { get; set; }
        public int proc_total { get; set; }
        public string rates_mode { get; set; }
        public int uptime { get; set; }
        public int run_queue { get; set; }
        public int processors { get; set; }
        public List<ExchangeType> exchange_types { get; set; }
        public List<AuthMechanism> auth_mechanisms { get; set; }
        public List<Application> applications { get; set; }
        public List<Context> contexts { get; set; }
        public List<string> log_files { get; set; }
        public string db_dir { get; set; }
        public List<string> config_files { get; set; }
        public int net_ticktime { get; set; }
        public List<string> enabled_plugins { get; set; }
        public string mem_calculation_strategy { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool running { get; set; }
        public int mem_used { get; set; }
        public ThroughputDetails mem_used_details { get; set; }
        public int fd_used { get; set; }
        public ThroughputDetails fd_used_details { get; set; }
        public int sockets_used { get; set; }
        public ThroughputDetails sockets_used_details { get; set; }
        public int proc_used { get; set; }
        public ThroughputDetails proc_used_details { get; set; }
        public long disk_free { get; set; }
        public ThroughputDetails disk_free_details { get; set; }
        public int gc_num { get; set; }
        public ThroughputDetails gc_num_details { get; set; }
        public long gc_bytes_reclaimed { get; set; }
        public ThroughputDetails gc_bytes_reclaimed_details { get; set; }
        public int context_switches { get; set; }
        public ThroughputDetails context_switches_details { get; set; }
        public int io_read_count { get; set; }
        public ThroughputDetails io_read_count_details { get; set; }
        public int io_read_bytes { get; set; }
        public ThroughputDetails io_read_bytes_details { get; set; }
        public double io_read_avg_time { get; set; }
        public ThroughputDetails io_read_avg_time_details { get; set; }
        public int io_write_count { get; set; }
        public ThroughputDetails io_write_count_details { get; set; }
        public int io_write_bytes { get; set; }
        public ThroughputDetails io_write_bytes_details { get; set; }
        public double io_write_avg_time { get; set; }
        public ThroughputDetails io_write_avg_time_details { get; set; }
        public int io_sync_count { get; set; }
        public ThroughputDetails io_sync_count_details { get; set; }
        public double io_sync_avg_time { get; set; }
        public ThroughputDetails io_sync_avg_time_details { get; set; }
        public int io_seek_count { get; set; }
        public ThroughputDetails io_seek_count_details { get; set; }
        public double io_seek_avg_time { get; set; }
        public ThroughputDetails io_seek_avg_time_details { get; set; }
        public int io_reopen_count { get; set; }
        public ThroughputDetails io_reopen_count_details { get; set; }
        public int mnesia_ram_tx_count { get; set; }
        public ThroughputDetails mnesia_ram_tx_count_details { get; set; }
        public int mnesia_disk_tx_count { get; set; }
        public ThroughputDetails mnesia_disk_tx_count_details { get; set; }
        public int msg_store_read_count { get; set; }
        public ThroughputDetails msg_store_read_count_details { get; set; }
        public int msg_store_write_count { get; set; }
        public ThroughputDetails msg_store_write_count_details { get; set; }
        public int queue_index_journal_write_count { get; set; }
        public ThroughputDetails queue_index_journal_write_count_details { get; set; }
        public int queue_index_write_count { get; set; }
        public ThroughputDetails queue_index_write_count_details { get; set; }
        public int queue_index_read_count { get; set; }
        public ThroughputDetails queue_index_read_count_details { get; set; }
        public int io_file_handle_open_attempt_count { get; set; }
        public ThroughputDetails io_file_handle_open_attempt_count_details { get; set; }
        public double io_file_handle_open_attempt_avg_time { get; set; }
        public ThroughputDetails io_file_handle_open_attempt_avg_time_details { get; set; }

        public List<object> cluster_links { get; set; }
        public MetricsGcQueueLength metrics_gc_queue_length { get; set; }
    }
}
