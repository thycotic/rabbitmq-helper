using System;
using System.Collections.Generic;
using Thycotic.MemoryMq;
using Thycotic.MessageQueueClient.Wrappers;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq model
    /// </summary>
    public class MemoryMqModel : ICommonModel
    {
        private readonly IMemoryMqServer _server;
        private readonly MemoryMqServiceCallback _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqModel"/> class.
        /// </summary>
        /// <param name="server">The create channel.</param>
        /// <param name="callback"></param>
        public MemoryMqModel(IMemoryMqServer server, MemoryMqServiceCallback callback)
        {
            _server = server;
            _callback = callback;
        }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue { get { return null; } }


        #region Mapping
        private ICommonModelProperties Map(MemoryMqProperties properties)
        {
            return new MemoryMqModelProperties(properties);
        }
        #endregion

        /// <summary>
        /// Creates the basic properties.
        /// </summary>
        /// <returns></returns>
        public ICommonModelProperties CreateBasicProperties()
        {
            return new MemoryMqModelProperties(new MemoryMqProperties());
        }

        /// <summary>
        /// Confirms the select.
        /// </summary>
        public void ConfirmSelect()
        {
            //nothing here
        }

        /// <summary>
        /// Exchanges the declare.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        public void ExchangeDeclare(string exchangeName, string exchangeType)
        {
            //nothing here
        }

        /// <summary>
        /// Basics the publish.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="immediate">if set to <c>true</c> [do not deliver immediately or require a listener].</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate,
            ICommonModelProperties properties, byte[] body)
        {
            _server.BasicPublish(exchangeName, routingKey, mandatory, immediate, properties.GetRawValue<MemoryMqProperties>(), body);

        }

        /// <summary>
        /// Waits for confirms or die.
        /// </summary>
        /// <param name="confirmationTimeout">The confirmation timeout.</param>
        public void WaitForConfirmsOrDie(TimeSpan confirmationTimeout)
        {
            //nothing here
        }

        /// <summary>
        /// Basic quality of service.
        /// </summary>
        /// <param name="prefetchSize">Size of the prefetch.</param>
        /// <param name="prefetchCount">The prefetch count.</param>
        /// <param name="global">if set to <c>true</c> global.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            //nothing here
        }

        /// <summary>
        /// Gets or sets the model shutdown.
        /// </summary>
        /// <value>
        /// The model shutdown.
        /// </value>
        public EventHandler<ModelShutdownEventArgs> ModelShutdown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Basics the ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            _server.BasicAck(deliveryTag, multiple);
        }

        /// <summary>
        /// Basics the nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            _server.BasicNack(deliveryTag, multiple);
        }

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <returns></returns>
        public ICommonQueue QueueDeclare()
        {
            var queueName = Guid.NewGuid().ToString();
            return new MemoryMqQueue(queueName);
        }

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="durable">if set to <c>true</c> [durable].</param>
        /// <param name="exclusive">if set to <c>true</c> [exclusive].</param>
        /// <param name="autoDelete">if set to <c>true</c> [automatic delete].</param>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICommonQueue QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            return new MemoryMqQueue(queueName);
        }

        /// <summary>
        /// Queues the bind.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            _server.QueueBind(queueName, exchangeName, routingKey);
        }

        /// <summary>
        /// Creates the subscription to the model.
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ISubscription CreateSubscription(string queueName)
        {
            return new MemoryMqSubscription(queueName, _server, _callback);
        }

        /// <summary>
        /// Basics the consume.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="consumer">The consumer.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void BasicConsume(string queueName, bool noAck, IConsumerWrapperBase consumer)
        {
            //tell the server we want to consume
            _server.BasicConsume(queueName);

            //when the server sends us something, process it
            _callback.BytesReceived +=
                (sender, deliveryArgs) =>
                    consumer.HandleBasicDeliver(deliveryArgs.ConsumerTag, deliveryArgs.DeliveryTag, deliveryArgs.Redelivered, deliveryArgs.Exchange,
                        deliveryArgs.RoutingKey, Map(deliveryArgs.Properties), deliveryArgs.Body);

        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Close()
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}