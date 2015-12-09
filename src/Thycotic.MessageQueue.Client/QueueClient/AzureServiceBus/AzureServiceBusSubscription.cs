using System;
using System.Diagnostics.Contracts;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
using Thycotic.MessageQueue.Client.Wrappers;

namespace Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus
{
    /// <summary>
    /// Memory Mq subscription
    /// </summary>
    public class AzureServiceBusSubscription : ISubscription
    {
        private readonly string _queueName;
        private readonly IAzureServiceBusConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqSubscription" /> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="connection"></param>
        public AzureServiceBusSubscription(string queueName, IAzureServiceBusConnection connection)
        {
            Contract.Requires<ArgumentNullException>(queueName != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
            Contract.Requires<ArgumentNullException>(connection != null);

            _queueName = queueName;
            _connection = connection;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            var manager = _connection.CreateManager();

            manager.DeleteQueueAsync(_queueName);


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
        public bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response)
        {
            var requestClient = _connection.CreateQueueClient(_queueName);
            
            var message = requestClient.Receive(TimeSpan.FromMilliseconds(timeoutMilliseconds));

            if (message == null)
            {
                response = null;
                return false;
            }
            else
            {
                var properties = new AzureServiceBusModelProperties(message);

                response = new CommonDeliveryEventArgs(string.Empty, new DeliveryTagWrapper(message.LockToken),
                    message.DeliveryCount > 0, properties.Exchange,
                    properties.RoutingKey, properties, message.GetBytes());

                return true;
            }
        }
    }
}