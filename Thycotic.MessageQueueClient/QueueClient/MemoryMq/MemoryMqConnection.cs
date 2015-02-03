using System;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq connection
    /// </summary>
    public class MemoryMqConnection : IConnection
    {
        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        public EventHandler ConnectionCreated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqConnection"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public MemoryMqConnection(string url)
        {

        }


        /// <summary>
        /// Forces the initialization.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool ForceInitialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        public IModel OpenChannel(int retryAttempts, int retryDelayMs, int retryDelayGrowthFactor)
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