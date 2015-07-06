using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Interface for a model subscription
    /// </summary>
    [ContractClass(typeof(SubscriptionContract))]
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
        /// Next
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="response">The <see cref="CommonDeliveryEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response);
    }

    /// <summary>
    /// Contract for ISubsrciption
    /// </summary>
    [ContractClassFor(typeof(ISubscription))]
    public abstract class SubscriptionContract : ISubscription
    {
        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; private set; }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="response">The <see cref="CommonDeliveryEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response)
        {
            Contract.ValueAtReturn(out response);
            return default(bool);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}