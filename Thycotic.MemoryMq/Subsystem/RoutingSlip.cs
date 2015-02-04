namespace Thycotic.MemoryMq.Subsystem
{
    public class RoutingSlip
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }

        public RoutingSlip(string exchangeName, string routingKey)
        {
            Exchange = exchangeName;
            RoutingKey = routingKey;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            
            var other = (RoutingSlip)obj;
            return Exchange == other.Exchange && RoutingKey == other.RoutingKey;

        }

        public override int GetHashCode()
        {
            return Exchange.GetHashCode() ^ RoutingKey.GetHashCode();
        }
    }
}