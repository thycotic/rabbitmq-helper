using System;
using RabbitMQ.Client.Events;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq subscription
    /// </summary>
    public class MemoryMqSubscription : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqSubscription"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="queueName">Name of the queue.</param>
        public MemoryMqSubscription(IMemoryMqModel channel, object queueName)
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
        /// <param name="response">The <see cref="BasicDeliverEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public bool Next(int timeoutMilliseconds, out BasicDeliverEventArgs response)
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