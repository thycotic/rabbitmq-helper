using System;
using RabbitMQ.Client.Events;
using Thycotic.MemoryMq;
using Thycotic.MessageQueueClient.QueueClient.RabbitMq;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq subscription
    /// </summary>
    public class MemoryMqSubscription : ISubscription
    {
        private readonly IMemoryMqServer _server;
        private readonly MemoryMqServiceCallback _callback;
        private readonly string _queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqSubscription" /> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="server">The model.</param>
        /// <param name="callback">The callback.</param>
         public MemoryMqSubscription(string queueName, IMemoryMqServer server, MemoryMqServiceCallback callback)
        {
            _server = server;
            _callback = callback;
            _queueName = queueName;
        }

        public void Dispose()
        {
        }

        public string QueueName {get { return _queueName; }
        }
        public bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response)
        {
            _server.QueueBind(_queueName, string.Empty, string.Empty);

            _server.BasicConsume(_queueName);

            //TODO: Keep working on this.

            //var received = false;

            ////when the server sends us something, process it
            //_callback.BytesReceived += (sender, deliveryArgs) =>
            //{
                
            //};

            //response = null;
            //MemoryQueueDeliveryEventArgs eventArgs;

            //_server.
            //if (!_subscription.Next(timeoutMilliseconds, out eventArgs))
            //{
            //    return false;
            //}

            //response = new CommonDeliveryEventArgs(eventArgs.ConsumerTag, eventArgs.DeliveryTag, eventArgs.Redelivered, eventArgs.Exchange,
            //    eventArgs.RoutingKey, new MemoryMqModelProperties(eventArgs.Properties), eventArgs.Body);
            return true;
        }
        }
    }
}