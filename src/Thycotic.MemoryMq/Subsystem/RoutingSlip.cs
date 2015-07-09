using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Routing slip
    /// </summary>
    public class RoutingSlip
    {
        /// <summary>
        /// Gets the exchange.
        /// </summary>
        /// <value>
        /// The exchange.
        /// </value>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets the routing key.
        /// </summary>
        /// <value>
        /// The routing key.
        /// </value>
        public string RoutingKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutingSlip"/> class.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        public RoutingSlip(string exchangeName, string routingKey)
        {
            Exchange = exchangeName;
            RoutingKey = routingKey;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(RoutingSlip other)
        {
            Contract.Requires<ArgumentNullException>(other != null);
            return string.Equals(RoutingKey, other.RoutingKey) && string.Equals(Exchange, other.Exchange);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RoutingSlip)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((RoutingKey != null ? RoutingKey.GetHashCode() : 0) * 397) ^ (Exchange != null ? Exchange.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(RoutingSlip left, RoutingSlip right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(RoutingSlip left, RoutingSlip right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(RoutingKey))
            {
                throw new ApplicationException("No routing key");
            }

            return !string.IsNullOrWhiteSpace(Exchange)
                ? string.Format("{0}:{1}", Exchange, RoutingKey)
                : RoutingKey;
        }
    }
}