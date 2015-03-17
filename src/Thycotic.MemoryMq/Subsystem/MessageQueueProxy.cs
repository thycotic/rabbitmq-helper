using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Queue proxy. Limits the ability to queuing any new items
    /// </summary>
    public class MessageQueueProxy
    {
        private readonly IMessageQueue _queue;

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
        /// Gets a value indicating whether this instance has unacknowledged.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has unacknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool HasUnacknowledged
        {
            get { return _queue.HasUnacknowledged; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueProxy"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        public MessageQueueProxy(IMessageQueue queue)
        {
            Contract.Requires<ArgumentNullException>(queue != null);

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


        /// <summary>
        /// Negatively the acknoledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        public void NegativelyAcknoledge(ulong deliveryTag)
        {
            _queue.NegativelyAcknoledge(deliveryTag);
        }
    }
}