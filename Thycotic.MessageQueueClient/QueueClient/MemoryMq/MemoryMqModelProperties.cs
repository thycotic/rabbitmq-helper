namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq model properties
    /// </summary>
    public class MemoryMqModelProperties : ICommonModelProperties
    {
        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets if there is a ReplyTo in the properties
        /// </summary>
        /// <returns></returns>
        public bool IsReplyToPresent()
        {
            return !string.IsNullOrWhiteSpace(ReplyTo);
        }

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

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue
        {
            get { return this; }
        }
    }
}