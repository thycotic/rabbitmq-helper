namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a message queue
    /// </summary>
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
        /// Negatively the acknoledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        void NegativelyAcknoledge(ulong deliveryTag);
    }
}