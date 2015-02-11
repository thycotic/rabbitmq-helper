namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Interface for an object that has a raw value
    /// </summary>
    public interface IHasRawValue
    {
        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        object RawValue { get;  }
    }
}