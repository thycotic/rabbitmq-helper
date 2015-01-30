using System;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Interface for a memory message queue connection
    /// </summary>
    public interface IMemoryMqConnection : IDisposable
    {
        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <returns></returns>
        IMemoryMqModel OpenChannel();

        /// <summary>
        /// Creates the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        void CreateQueue(string queueName);

        /// <summary>
        /// Gets the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        IMemoryMqVolatileQueue GetQueue(string queueName);
    }
}
