using System;
using Thycotic.MessageQueueClient.Wrappers;

namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Interface for a Memory Mq model
    /// </summary>
    public interface IModel : IDisposable
    {
        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <returns></returns>
        IQueue QueueDeclare();

        /// <summary>
        /// Creates the basic properties.
        /// </summary>
        /// <returns></returns>
        IModelProperties CreateBasicProperties();

        /// <summary>
        /// Confirms the select.
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
        /// <param name="doNotDeliverImmediatelyOrRequireAListener">if set to <c>true</c> [do not deliver immediately or require a listener].</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool doNotDeliverImmediatelyOrRequireAListener, IModelProperties properties, byte[] body);

        /// <summary>
        /// Waits for confirms or die.
        /// </summary>
        /// <param name="confirmationTimeout">The confirmation timeout.</param>
        void WaitForConfirmsOrDie(TimeSpan confirmationTimeout);

        /// <summary>
        /// Basic quality of service.
        /// </summary>
        /// <param name="prefetchSize">Size of the prefetch.</param>
        /// <param name="prefetchCount">The prefetch count.</param>
        /// <param name="global">if set to <c>true</c> global.</param>
        void BasicQos(int prefetchSize, int prefetchCount, bool global);

        /// <summary>
        /// Gets or sets the model shutdown.
        /// </summary>
        /// <value>
        /// The model shutdown.
        /// </value>
        EventHandler<ModelShutdownEventArgs> ModelShutdown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        bool IsOpen { get; set; }

        /// <summary>
        /// Basics the ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        void BasicAck(ulong deliveryTag, bool multiple);

        /// <summary>
        /// Basics the nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        void BasicNack(ulong deliveryTag, bool multiple, bool requeue);

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="b">if set to <c>true</c> [b].</param>
        /// <param name="b1">if set to <c>true</c> [b1].</param>
        /// <param name="b2">if set to <c>true</c> [b2].</param>
        /// <param name="o">The o.</param>
        void QueueDeclare(string queueName, bool b, bool b1, bool b2, object o);

        /// <summary>
        /// Queues the bind.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        void QueueBind(string queueName, string exchange, string routingKey);

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