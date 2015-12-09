using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.MemoryMq;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf;
using Thycotic.MessageQueue.Client.Wrappers;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq subscription
    /// </summary>
    public class MemoryMqSubscription : ISubscription
    {
        private readonly IMemoryMqWcfService _server;
        private readonly MemoryMqWcfServiceCallback _callback;
        private readonly string _queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqSubscription" /> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="server">The model.</param>
        /// <param name="callback">The callback.</param>
        public MemoryMqSubscription(string queueName, IMemoryMqWcfService server, MemoryMqWcfServiceCallback callback)
        {
            Contract.Requires<ArgumentNullException>(queueName != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
            Contract.Requires<ArgumentNullException>(server != null);
            Contract.Requires<ArgumentNullException>(callback != null);

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
        public string QueueName
        {
            get { return _queueName; }
        }
        /// <summary>
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <param name="response">The <see cref="CommonDeliveryEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        //TODO: disabling contract because the eventArgs is set in an event and static checker is confused -dkk
        [ContractVerification(false)]
        public bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response)
        {
            MemoryMqDeliveryEventArgs eventArgs = null;

            var cts = new CancellationTokenSource();

            //when the server sends us something, process it
            _callback.BytesReceived += (sender, deliveryArgs) =>
            {
                eventArgs = deliveryArgs;
                cts.Cancel();
            };

            var routingKey = _queueName;

            //because _queueName already has this contract
            Contract.Assume(!string.IsNullOrWhiteSpace(routingKey));

            _server.QueueBind(_queueName, string.Empty, routingKey);

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
            else
            {
                _server.BasicAck(eventArgs.DeliveryTag, eventArgs.Exchange, eventArgs.RoutingKey, false);

                response = new CommonDeliveryEventArgs(eventArgs.ConsumerTag, new DeliveryTagWrapper(eventArgs.DeliveryTag),
                    eventArgs.Redelivered, eventArgs.Exchange,
                    eventArgs.RoutingKey, new MemoryMqModelProperties(eventArgs.Properties), eventArgs.Body);

                return true;
            }

        }
    }
}