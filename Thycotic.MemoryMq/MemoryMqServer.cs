using System;
using System.ServiceModel;
using Thycotic.Logging;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Memory mq server
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class MemoryMqServer : IMemoryMqServer, IDisposable
    {
        private readonly ExchangeDictionary _messages = new ExchangeDictionary();
        private readonly BindingDictionary _bindings = new BindingDictionary();
        private readonly ClientDictionary _clientDictionary = new ClientDictionary();
        private readonly MessageDispatcher _messageDispatcher;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServer));

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServer"/> class.
        /// </summary>
        public MemoryMqServer()
        {
            _messageDispatcher = new MessageDispatcher(_messages, _bindings, _clientDictionary);
            _messageDispatcher.Start();
        }

        /// <summary>
        /// Basic publish.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="immediate">if set to <c>true</c> [immediate].</param>
        /// <param name="properties"></param>
        /// <param name="body">The body.</param>
        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, MemoryMqProperties properties, byte[] body)
        {
            _messages.Publish(new RoutingSlip(exchangeName, routingKey), new MemoryQueueDeliveryEventArgs(exchangeName, routingKey, properties, body));
        }

        /// <summary>
        /// Binds a queue to an exchange and a routing key
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            _bindings.AddBinding(new RoutingSlip(exchangeName, routingKey), queueName);
        }

        /// <summary>
        /// Basic consume
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void BasicConsume(string queueName)
        {
            _log.Debug("Attaching consumer");
            
            _clientDictionary.AddClient(queueName);
        }

        /// <summary>
        /// Basic ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            //TODO: Implement
        }

        /// <summary>
        /// Basic nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        public void BasicNack(ulong deliveryTag, bool multiple)
        {
            //TODO: Implement
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _messageDispatcher.Stop();
        }
    }
}