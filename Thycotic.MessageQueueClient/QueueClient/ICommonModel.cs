using System;
using System.Collections.Generic;
using Thycotic.MessageQueueClient.Wrappers;

namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Interface for a Memory Mq model
    /// </summary>
    public interface ICommonModel : IHasRawValue, IDisposable
    {

        /// <summary>
        /// Creates the basic properties.
        /// </summary>
        /// <returns></returns>
        ICommonModelProperties CreateBasicProperties();

        /// <summary>
        /// Enable publisher acknowledgements.
        /// </summary>
        void ConfirmSelect();

        /// <summary>
        /// Exchanges the declare.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        void ExchangeDeclare(string exchangeName, string exchangeType);

        /// <summary>
        /// Basics the publish.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="immediate">if set to <c>true</c> [do not deliver immediately or require a listener].</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, ICommonModelProperties properties, byte[] body);

        /// <summary>
        /// Wait until all published messages have been confirmed.
        /// </summary>
        /// <param name="confirmationTimeout">The confirmation timeout.</param>
        void WaitForConfirmsOrDie(TimeSpan confirmationTimeout);

        /// <summary>
        /// Basic quality of service.
        /// </summary>
        /// <param name="prefetchSize">Size of the prefetch.</param>
        /// <param name="prefetchCount">The prefetch count.</param>
        /// <param name="global">if set to <c>true</c> global.</param>
        void BasicQos(uint prefetchSize, ushort prefetchCount, bool global);

        /// <summary>
        /// Gets or sets the model shutdown.
        /// </summary>
        /// <value>
        /// The model shutdown.
        /// </value>
        EventHandler<ModelShutdownEventArgs> ModelShutdown { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        bool IsOpen { get; }

        /// <summary>
        /// Basics the ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        void BasicAck(ulong deliveryTag, string exchange, string routingKey, bool multiple);

        /// <summary>
        /// Basics the nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        void BasicNack(ulong deliveryTag, string exchange, string routingKey, bool multiple, bool requeue);

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <returns></returns>
        ICommonQueue QueueDeclare();

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="durable">if set to <c>true</c> durable.</param>
        /// <param name="exclusive">if set to <c>true</c> exclusive.</param>
        /// <param name="autoDelete">if set to <c>true</c> auto delete.</param>
        /// <param name="arguments">The arguments.</param>
        ICommonQueue QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments);

        /// <summary>
        /// Queues the bind.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        void QueueBind(string queueName, string exchangeName, string routingKey);

        /// <summary>
        /// Creates the subscription to the model.
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ISubscription CreateSubscription(string queueName);

        /// <summary>
        /// Basics the consume.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="consumer">The consumer.</param>
        void BasicConsume(string queueName, bool noAck, IConsumerWrapperBase consumer);

        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();

        
    }
}