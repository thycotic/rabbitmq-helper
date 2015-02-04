using System.Collections.Concurrent;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Queue proxy. Limits the ability to queuing any new items
    /// </summary>
    public class QueueProxy
    {
        private readonly ConcurrentQueue<MemoryQueueDeliveryEventArgs> _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProxy"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        public QueueProxy(ConcurrentQueue<MemoryQueueDeliveryEventArgs> queue)
        {
            _queue = queue;
        }

        /// <summary>
        /// Tries to deque.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public bool TryDequeue(out MemoryQueueDeliveryEventArgs body)
        {
            return _queue.TryDequeue(out body);
        }
    }
}