using System.Collections.Generic;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591

    public class ComponentParameters
    {
        public string component { get; set; }
        public string name { get; set; }
        public IDictionary<string, object> value { get; set; }
    }
#pragma warning restore 1591


}