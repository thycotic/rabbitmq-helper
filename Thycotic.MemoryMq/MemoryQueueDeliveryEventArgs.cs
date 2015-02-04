using System.Runtime.Serialization;

namespace Thycotic.MemoryMq
{
    [DataContract]
    public class MemoryQueueDeliveryEventArgs
    {
        /// <summary>
        /// Gets or sets the consumer tag.
        /// </summary>
        /// <value>
        /// The consumer tag.
        /// </value>
        [DataMember]
        public string ConsumerTag { get; set; }

        /// <summary>
        /// Gets or sets the delivery tag.
        /// </summary>
        /// <value>
        /// The delivery tag.
        /// </value>
        [DataMember]
        public ulong DeliveryTag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MemoryQueueDeliveryEventArgs"/> is redelivered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if redelivered; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Redelivered { get; set; }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>
        /// The exchange.
        /// </value>
        [DataMember]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the routing key.
        /// </summary>
        /// <value>
        /// The routing key.
        /// </value>
        [DataMember]
        public string RoutingKey { get; set; }
        
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [DataMember]
        public byte[] Body { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryQueueDeliveryEventArgs" /> class.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="body">The body.</param>
        public MemoryQueueDeliveryEventArgs(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, byte[] body)
        {
            ConsumerTag = consumerTag;
            DeliveryTag = deliveryTag;
            Redelivered = redelivered;
            Exchange = exchange;
            RoutingKey = routingKey;
            Body = body;
        }
    }
}