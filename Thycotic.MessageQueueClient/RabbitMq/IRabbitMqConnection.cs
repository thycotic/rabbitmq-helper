using System;
using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    /// <summary>
    /// Interface for Rabbit Mq connections
    /// </summary>
    public interface IRabbitMqConnection
    {
        /// <summary>
        /// Occurs when a connection is created.
        /// </summary>
        event EventHandler ConnectionCreated;

        /// <summary>
        /// Forces the initialization.
        /// </summary>
        bool ForceInitialize();

        /// <summary>
        /// Opens the channel with the specified retry attempts, retry delay and retry delay growth factor
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        IModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor);
    }
}
