using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591

    public class Policy
    {
        public string pattern { get; set; }
        public IDictionary<string,object> definition { get; set; }
        public int priority { get; set; }

        [JsonProperty(PropertyName = "apply-to")]
        public string apply_to { get; set; }
}
#pragma warning restore 1591


}