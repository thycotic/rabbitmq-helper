using System;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Interface for a Memory Mq connection
    /// </summary>
    public interface IMemoryMqConnection : IDisposable
    {
        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        IMemoryMqModel OpenChannel(int retryAttempts, int retryDelayMs, int retryDelayGrowthFactor);
    }
}