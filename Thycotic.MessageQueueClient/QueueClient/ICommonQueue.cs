namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Interface for a Memory Mq queue
    /// </summary>
    public interface ICommonQueue
    {
        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        object QueueName { get;  }
    }
}