namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Deliver event arguments
    /// </summary>
    public class CommonDeliveryEventArgs
    {
        /// <summary>
        /// Gets or sets the consumer tag.
        /// </summary>
        /// <value>
        /// The consumer tag.
        /// </value>
        public string ConsumerTag { get; set; }

        /// <summary>
        /// Gets or sets the delivery tag.
        /// </summary>
        /// <value>
        /// The delivery tag.
        /// </value>
        public ulong DeliveryTag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommonDeliveryEventArgs"/> is redelivered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if redelivered; otherwise, <c>false</c>.
        /// </value>
        public bool Redelivered { get; set; }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>
        /// The exchange.
        /// </value>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the routing key.
        /// </summary>
        /// <value>
        /// The routing key.
        /// </value>
        public string RoutingKey { get; set; }
        
        /// <summary>
        /// Gets or sets the basic properties.
        /// </summary>
        /// <value>
        /// The basic properties.
        /// </value>
        public ICommonModelProperties BasicProperties { get; set; }
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public byte[] Body { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonDeliveryEventArgs"/> class.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        public CommonDeliveryEventArgs(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            ICommonModelProperties properties, byte[] body)
        {
            ConsumerTag = consumerTag;
            DeliveryTag = deliveryTag;
            Redelivered = redelivered;
            Exchange = exchange;
            RoutingKey = routingKey;
            BasicProperties = properties;
            Body = body;
        }

        
    }
}