using System.Collections.Concurrent;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory message queue connection
    /// </summary>
    public class MemoryMqConnection : IMemoryMqConnection
    {
        private readonly ConcurrentDictionary<string, MemoryMqVolatileQueue> _queues = new ConcurrentDictionary<string, MemoryMqVolatileQueue>();

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <returns></returns>
        public IMemoryMqModel OpenChannel()
        {
            return new MemoryMqModel(this);
        }

        /// <summary>
        /// Creates the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void CreateQueue(string queueName)
        {
            //does nothing since queues are created dynamically
        }

        /// <summary>
        /// Gets the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public IMemoryMqVolatileQueue GetQueue(string queueName)
        {
            return _queues.GetOrAdd(queueName, qn => new MemoryMqVolatileQueue());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _queues.Clear();
        }
    }
}