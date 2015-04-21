namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a message queue proxy
    /// </summary>
    public interface IMessageQueueProxy
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
        /// Tries to deque.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        bool TryDequeue(out MemoryMqDeliveryEventArgs body);

        /// <summary>
        /// Negatively the acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        void NegativelyAcknoledge(ulong deliveryTag, bool requeue);
    }
}