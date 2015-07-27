using System;
using System.Diagnostics.Contracts;
using Thycotic.Utility.TestChain;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Interface for a Memory Mq connection
    /// </summary>
    [UnitTestsRequired]
    [ContractClass(typeof(CommonConnectionContract))]
    public interface ICommonConnection : IDisposable
    {
        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        EventHandler ConnectionCreated { get; set; }

        /// <summary>
        /// Holds the Queue version retrieved from the server.
        /// </summary>
        Version ServerVersion { get; }

        /// <summary>
        /// Forces the initialization.
        /// </summary>
        void ResetConnection();

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        ICommonModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor);
    }

    /// <summary>
    /// Contract for ICommonConnection
    /// </summary>
    [ContractClassFor(typeof(ICommonConnection))]
    public abstract class CommonConnectionContract : ICommonConnection
    {
        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        public EventHandler ConnectionCreated { get; set; }

        /// <summary>
        /// Holds the Queue Server version retrieved from the server.
        /// </summary>
        public Version ServerVersion { get { return null; } }

        /// <summary>
        /// Forces the initialization.
        /// </summary>
        public void ResetConnection()
        {
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        public ICommonModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor)
        {
            Contract.Ensures(Contract.Result<ICommonModel>() != null);

            return default(ICommonModel);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}