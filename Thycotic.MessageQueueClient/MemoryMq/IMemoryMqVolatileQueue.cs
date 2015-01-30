using System.Collections.Concurrent;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Interface for a memory message queue volatile queue
    /// </summary>
    public interface IMemoryMqVolatileQueue
    {
        /// <summary>
        /// Enqueues the specified consumable.
        /// </summary>
        /// <param name="consumable">The consumable.</param>
        /// <returns></returns>
        void Enqueue(byte[] consumable);

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="consumable">The consumable.</param>
        /// <returns></returns>
        bool TryDequeue(out byte[] consumable);
    }
}