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

        protected bool Equals(RoutingSlip other)
        {
            return string.Equals(RoutingKey, other.RoutingKey) && string.Equals(Exchange, other.Exchange);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RoutingSlip)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((RoutingKey != null ? RoutingKey.GetHashCode() : 0) * 397) ^ (Exchange != null ? Exchange.GetHashCode() : 0);
            }
        }

        public static bool operator ==(RoutingSlip left, RoutingSlip right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RoutingSlip left, RoutingSlip right)
        {
            return !Equals(left, right);
        }
    }
}