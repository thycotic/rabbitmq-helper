namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq connection
    /// </summary>
    public class MemoryMqConnection : IMemoryMqConnection
    {
        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        public IMemoryMqModel OpenChannel(int retryAttempts, int retryDelayMs, int retryDelayGrowthFactor)
        {
            return new MemoryMqModel();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}