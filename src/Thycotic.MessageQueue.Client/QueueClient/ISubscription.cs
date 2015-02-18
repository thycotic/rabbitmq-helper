using System;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Interface for a model subscription
    /// </summary>
    public interface ISubscription : IDisposable
    {
        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        string QueueName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="response">The <see cref="CommonDeliveryEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response);
    }
}