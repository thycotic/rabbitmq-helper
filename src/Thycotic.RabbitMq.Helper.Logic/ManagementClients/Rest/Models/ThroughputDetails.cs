using System.Globalization;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
#pragma warning disable 1591
    public class ThroughputDetails
    {
        public double rate { get; set; }

        public override string ToString()
        {
            return rate.ToString(CultureInfo.InvariantCulture);
        }
    }
#pragma warning restore 1591
}