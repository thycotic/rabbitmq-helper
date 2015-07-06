using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a message queue
    /// </summary>
    [ContractClass(typeof(MessageQueueContract))]
    public interface IMessageQueue
    {
        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has unacknowledged.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has unacknowledged; otherwise, <c>false</c>.
        /// </value>
        bool HasUnacknowledged { get; }

        /// <summary>
        /// Enqueues the specified body.
        /// </summary>
        /// <param name="body">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        void Enqueue(MemoryMqDeliveryEventArgs body);

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="body">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        bool TryDequeue(out MemoryMqDeliveryEventArgs body);

        /// <summary>
        /// Acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <exception cref="System.ApplicationException"></exception>
        void Acknowledge(ulong deliveryTag);

        /// <summary>
        /// Negatively acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        void NegativelyAcknoledge(ulong deliveryTag, bool requeue);

        /// <summary>
        /// Negatively acknowledge all pending delivery tags.
        /// </summary>
        void NegativelyAcknoledgeAllPending();
    }

    /// <summary>
    /// Contract for IMessageQueue
    /// </summary>
    [ContractClassFor(typeof (IMessageQueue))]
    public abstract class MessageQueueContract : IMessageQueue
    {
        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has unacknowledged.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has unacknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool HasUnacknowledged { get; private set; }

        /// <summary>
        /// Enqueues the specified body.
        /// </summary>
        /// <param name="body">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        public void Enqueue(MemoryMqDeliveryEventArgs body)
        {
            Contract.Requires<ArgumentNullException>(body != null);
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="body">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public bool TryDequeue(out MemoryMqDeliveryEventArgs body)
        {
            Contract.ValueAtReturn(out body);
            return default(bool);
        }

        /// <summary>
        /// Acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Acknowledge(ulong deliveryTag)
        {
        }

        /// <summary>
        /// Negatively acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>

        public void NegativelyAcknoledge(ulong deliveryTag, bool requeue)
        {
        }

        /// <summary>
        /// Negatively acknowledge all pending delivery tags.
        /// </summary>
        public void NegativelyAcknoledgeAllPending()
        {
        }
    }

}