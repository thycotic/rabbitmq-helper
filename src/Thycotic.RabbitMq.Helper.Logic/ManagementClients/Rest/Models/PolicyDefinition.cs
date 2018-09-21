using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{

#pragma warning disable 1591

    public class PolicyDefinition
    {
        [JsonProperty("federation-upstream-set")]   
        public string federation_upstream_set { get; set; }
    
        [JsonProperty("queue-master-locator")]
        public string queue_master_locator { get; set; }

        [JsonProperty("ha-mode")]
        public string ha_mode { get; set; }

        [JsonProperty("ha-params")]
        public object ha_params { get; set; }

        [JsonProperty("ha-sync-mode")]
        public string ha_sync_mode { get; set; }

        [JsonProperty("ha-sync-batch-size")]
        public int ha_sync_batch_size { get; set; }
    }



#pragma warning restore 1591
}