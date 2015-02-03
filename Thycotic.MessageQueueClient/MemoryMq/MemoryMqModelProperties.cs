namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq model properties
    /// </summary>
    public class MemoryMqModelProperties
    {
        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        public string ReplyTo { get; set; }

        /// <summary>
        /// Sets the persistent.
        /// </summary>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        public void SetPersistent(bool persistent)
        {
            //TODO: Implement persistance
        }
    }
}