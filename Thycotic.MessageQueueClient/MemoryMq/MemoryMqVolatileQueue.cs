using System.Collections.Concurrent;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory message queue volatile queue
    /// </summary>
    public class MemoryMqVolatileQueue : IMemoryMqVolatileQueue
    {
        private readonly ConcurrentQueue<byte[]> _queue = new ConcurrentQueue<byte[]>();

        /// <summary>
        /// Enqueues the specified consumable.
        /// </summary>
        /// <param name="consumable">The consumable.</param>
        /// <returns></returns>
        public void Enqueue(byte[] consumable)
        {
            _queue.Enqueue(consumable);
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="consumable">The consumable.</param>
        /// <returns></returns>
        public bool TryDequeue(out byte[] consumable)
        {
            return _queue.TryDequeue(out consumable);
        }
    }
}