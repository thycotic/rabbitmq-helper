using System;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq subscription
    /// </summary>
    public class MemoryMqSubscription : ISubscription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqSubscription"/> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public MemoryMqSubscription(string queueName)
        {
            QueueName = queueName;
        }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="response">The <see cref="CommonDeliveryEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response)
        {
            response = null;
            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            
        }
    }
}