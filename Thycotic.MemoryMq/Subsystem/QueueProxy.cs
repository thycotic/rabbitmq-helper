using System.Collections.Concurrent;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Queue proxy. Limits the ability to queuing any new items
    /// </summary>
    public class QueueProxy
    {
        private readonly ConcurrentQueue<MemoryMqDeliveryEventArgs> _queue;

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return _queue.IsEmpty;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProxy"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        public QueueProxy(ConcurrentQueue<MemoryMqDeliveryEventArgs> queue)
        {
            _queue = queue;
        }

        /// <summary>
        /// Tries to deque.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public bool TryDequeue(out MemoryMqDeliveryEventArgs body)
        {
            return _queue.TryDequeue(out body);
        }

        
    }
}