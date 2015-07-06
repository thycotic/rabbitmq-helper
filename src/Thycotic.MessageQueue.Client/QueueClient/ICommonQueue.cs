using System.Diagnostics.Contracts;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Interface for a Memory Mq queue
    /// </summary>
    [ContractClass(typeof(CommonQueueContract))]
    public interface ICommonQueue
    {
        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        string QueueName { get; }
    }

    /// <summary>
    /// Contract for ICommonQueue
    /// </summary>
    [ContractClassFor(typeof (ICommonQueue))]
    public abstract class CommonQueueContract : ICommonQueue
    {
        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; private set; }
    }
}