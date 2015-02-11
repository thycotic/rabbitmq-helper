using System;
using System.Collections.Concurrent;
using Thycotic.MemoryMq.Collections;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Message queue
    /// </summary>
    public class MessageQueue : IMessageQueue
    {
        private readonly ConcurrentPriorityQueue<MemoryMqDeliveryEventArgs> _queue =
            new ConcurrentPriorityQueue<MemoryMqDeliveryEventArgs>();

        private readonly ConcurrentDictionary<ulong, MemoryMqDeliveryEventArgs> _unackedMessages =
            new ConcurrentDictionary<ulong, MemoryMqDeliveryEventArgs>();

        private readonly object _syncRoot = new object();

        private ulong _currentDeliveryTag;

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get { return _queue.IsEmpty; }
        }

        private ulong GetNextDeliveryTag()
        {
            lock (_syncRoot)
            {
                return _currentDeliveryTag++;
            }
        }

        /// <summary>
        /// Adds the specified delivery tag.
        /// </summary>
        /// <param name="message">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        private void AddUnacknowledgedMessage(MemoryMqDeliveryEventArgs message)
        {
            var deliveryTag = GetNextDeliveryTag();
            if (!_unackedMessages.TryAdd(deliveryTag, message))
            {
                throw new ApplicationException(string.Format("Could not add delivery tag {0}", deliveryTag));
            }

            message.DeliveryTag = deliveryTag;

        }

        /// <summary>
        /// Enqueues the specified body.
        /// </summary>
        /// <param name="body">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        public void Enqueue(MemoryMqDeliveryEventArgs body)
        {
           _queue.Enqueue(body);
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="body">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public bool TryDequeue(out MemoryMqDeliveryEventArgs body)
        {
            if (_queue.TryDequeue(out body))
            {
                AddUnacknowledgedMessage(body);
                return true;
            }

            body = null;
            return false;
        }

        /// <summary>
        /// Acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Acknowledge(ulong deliveryTag)
        {
            MemoryMqDeliveryEventArgs eventArgs;
            if (!_unackedMessages.TryRemove(deliveryTag, out eventArgs))
            {
                throw new ApplicationException(string.Format("Failed to acknowledge delivery tag {0}", deliveryTag));
            }

            //just remove, nothing more is needed
        }

        /// <summary>
        /// Negatively the acknoledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        public void NegativelyAcknoledge(ulong deliveryTag)
        {
            MemoryMqDeliveryEventArgs eventArgs;
            if (!_unackedMessages.TryRemove(deliveryTag, out eventArgs))
            {
                throw new ApplicationException(string.Format("Failed to negatively acknowledge delivery tag {0}",
                    deliveryTag));
            }

            //requeue at the front of the list
            _queue.PriorityEnqueue(eventArgs);
        }
    }
}
