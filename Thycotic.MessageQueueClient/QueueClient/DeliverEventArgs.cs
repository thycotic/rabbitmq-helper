namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Deliver event arguments
    /// </summary>
    public class DeliverEventArgs
    {
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
    }
}