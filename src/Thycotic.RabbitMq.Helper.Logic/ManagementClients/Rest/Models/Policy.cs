using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591

    public class Policy
    {
        public string pattern { get; set; }
        public Dictionary<string,object> definition { get; set; }
        public int priority { get; set; }

        [DeserializeAs(Name = "apply-to")]
        public string applyTo { get; set; }
}
#pragma warning restore 1591


}