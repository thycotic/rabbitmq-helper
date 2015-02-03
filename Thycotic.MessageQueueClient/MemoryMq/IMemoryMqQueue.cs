namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Interface for a Memory Mq queue
    /// </summary>
    public interface IMemoryMqQueue
    {
        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        object QueueName { get; set; }
    }
}