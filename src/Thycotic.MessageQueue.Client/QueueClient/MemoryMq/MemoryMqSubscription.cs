using System;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.MemoryMq;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq subscription
    /// </summary>
    public class MemoryMqSubscription : ISubscription
    {
        private readonly IMemoryMqWcfServer _server;
        private readonly MemoryMqWcfServiceCallback _callback;
        private readonly string _queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqSubscription" /> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="server">The model.</param>
        /// <param name="callback">The callback.</param>
         public MemoryMqSubscription(string queueName, IMemoryMqWcfServer server, MemoryMqWcfServiceCallback callback)
        {
            _server = server;
            _callback = callback;
            _queueName = queueName;
        }

         /// <summary>
         /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
         /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName {get { return _queueName; }
        }
        /// <summary>
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="response">The <see cref="CommonDeliveryEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response)
        {
            var routingKey = _queueName;

            _server.QueueBind(_queueName, string.Empty, routingKey);

            MemoryMqDeliveryEventArgs eventArgs = null;

            var cts = new CancellationTokenSource();

            //when the server sends us something, process it
            _callback.BytesReceived += (sender, deliveryArgs) =>
            {
                eventArgs = deliveryArgs;
                cts.Cancel();
            };

            var task = Task.Factory.StartNew(() =>
            {
                _server.BasicConsume(_queueName);

                cts.Token.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMilliseconds));

            }, cts.Token);

            task.Wait(TimeSpan.FromMilliseconds(timeoutMilliseconds));

            if (!task.IsCompleted || eventArgs == null)
            {
                response = null;
                return false;
            }

            response = new CommonDeliveryEventArgs(eventArgs.ConsumerTag, eventArgs.DeliveryTag, eventArgs.Redelivered, eventArgs.Exchange,
                eventArgs.RoutingKey, new MemoryMqModelProperties(eventArgs.Properties), eventArgs.Body);
            
            return true;
        
        }
    }
}