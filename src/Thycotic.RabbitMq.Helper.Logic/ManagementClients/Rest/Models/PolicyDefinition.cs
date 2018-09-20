using RestSharp.Deserializers;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{

#pragma warning disable 1591

    public class PolicyDefinition
    {
    }

    public class FederationPolicyDefinition : PolicyDefinition
    {
        [DeserializeAs(Name = "federation-upstream-set")]
        public string federation_upstream_set { get; set; }
    }


    public class HaPolicyDefinition : PolicyDefinition
    {
        [DeserializeAs(Name = "queue-master-locator")]
        public string queue_master_locator { get; set; }

        [DeserializeAs(Name = "ha-mode")]
        public string ha_mode { get; set; }

        [DeserializeAs(Name = "ha-params")]
        public string ha_params { get; set; }

        [DeserializeAs(Name = "ha-sync-mode")]
        public string ha_sync_mode { get; set; }

        [DeserializeAs(Name = "ha-sync-batch-size")]
        public string ha_sync_batch_size { get; set; }
    }



#pragma warning restore 1591
}