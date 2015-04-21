using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Thycotic.MemoryMq.Collections;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Message queue
    /// </summary>
    public class MessageQueue : IMessageQueue
    {
        /// <summary>
        /// The maximum capacity count
        /// </summary>
        public const ulong MaxCapacityCount = 50*1000;

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
            get { return _queue.IsEmpty && _unackedMessages.IsEmpty; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has un-acknowledged.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has un-acknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool HasUnacknowledged
        {
            get { return !_unackedMessages.IsEmpty; }
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
            lock (_unackedMessages)
            {
                var deliveryTag = GetNextDeliveryTag();
                if (!_unackedMessages.TryAdd(deliveryTag, message))
                {
                    throw new ApplicationException(string.Format("Could not add delivery tag {0}", deliveryTag));
                }

                message.DeliveryTag = deliveryTag;
            }
        }

        /// <summary>
        /// Enqueues the specified body.
        /// </summary>
        /// <param name="body">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        public void Enqueue(MemoryMqDeliveryEventArgs body)
        {
            if (_queue.Count > MaxCapacityCount)
            {
                throw new InvalidOperationException("Cannot any more items due to capacity overflow");
            }

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
            lock (_unackedMessages)
            {
                MemoryMqDeliveryEventArgs eventArgs;
                if (!_unackedMessages.TryRemove(deliveryTag, out eventArgs))
                {
                    throw new ApplicationException(string.Format("Failed to acknowledge delivery tag {0}", deliveryTag));
                }

                //just remove, nothing more is needed
            }
        }

        /// <summary>
        /// Negatively the acknoledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="requeue"></param>
        public void NegativelyAcknoledge(ulong deliveryTag, bool requeue)
        {
            lock (_unackedMessages)
            {
                MemoryMqDeliveryEventArgs eventArgs;
                if (!_unackedMessages.TryRemove(deliveryTag, out eventArgs))
                {
                    throw new ApplicationException(string.Format("Failed to negatively acknowledge delivery tag {0}",
                        deliveryTag));
                }

                //no need to requeue
                if (!requeue) return;

                eventArgs.Redelivered = true;

                //requeue at the front of the list
                _queue.PriorityEnqueue(eventArgs);
            }
        }

        /// <summary>
        /// Negatively acknowledge all pending delivery tags.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void NegativelyAcknoledgeAllPending()
        {
            List<ulong> unackedDeliveryTags;

            lock (_unackedMessages)
            {
                unackedDeliveryTags = _unackedMessages.Keys.ToList();
            }

            unackedDeliveryTags.ForEach(dt =>
            {
                try
                {
                    NegativelyAcknoledge(dt, true);
                }
                catch (Exception)
                {
                    //couldn't nack
                }
            });
        }
    }
}
