using System;

namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Interface for a Memory Mq connection
    /// </summary>
    public interface IConnection : IDisposable
    {

        /// <summary>
        /// Forces the initialization.
        /// </summary>
        bool ForceInitialize();

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        IModel OpenChannel(int retryAttempts, int retryDelayMs, int retryDelayGrowthFactor);

        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        EventHandler ConnectionCreated { get; set; }

    }
}