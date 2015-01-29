using System;

namespace Thycotic.MessageQueueClient.Wrappers.RabbitMq
{
    /// <summary>
    /// Interface for a consumer wrapper base class
    /// </summary>
    public interface IConsumerWrapperBase : IDisposable
    {
        /// <summary>
        /// Starts the consuming.
        /// </summary>
        void StartConsuming();
    }
}