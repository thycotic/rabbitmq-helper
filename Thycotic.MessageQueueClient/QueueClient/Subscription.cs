using System;
using Thycotic.MessageQueueClient.QueueClient.MemoryMq;

namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Memory Mq subscription
    /// </summary>
    public class Subscription : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="queueName">Name of the queue.</param>
        public Subscription(IModel channel, object queueName)
        {

        }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Nexts the specified i.
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="response">The <see cref="DeliverEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public bool Next(int timeoutMilliseconds, out DeliverEventArgs response)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}