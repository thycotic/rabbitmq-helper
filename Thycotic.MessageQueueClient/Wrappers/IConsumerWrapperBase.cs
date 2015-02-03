using System;
using Thycotic.MessageQueueClient.QueueClient;

namespace Thycotic.MessageQueueClient.Wrappers
{
    /// <summary>
    /// Interface for a consumer wrapper base class
    /// </summary>
    public interface IConsumerWrapperBase : IDisposable
    {
        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        ICommonModel CommonModel { get; }

        /// <summary>
        /// Starts the consuming.
        /// </summary>
        void StartConsuming();
    }
}