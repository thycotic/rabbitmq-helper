using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Thycotic.MessageQueue.Client.Wrappers;
using Thycotic.Utility.TestChain;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Interface for a Memory Mq model
    /// </summary>
    [UnitTestsRequired]
    [ContractClass(typeof(CommonModelContract))]
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
        void BasicAck(DeliveryTagWrapper deliveryTag, string exchange, string routingKey, bool multiple);

        /// <summary>
        /// Basics the nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        void BasicNack(DeliveryTagWrapper deliveryTag, string exchange, string routingKey, bool multiple, bool requeue);

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
        /// <param name="token">The token.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="consumer">The consumer.</param>
        void BasicConsume(CancellationToken token, string queueName, bool noAck, IConsumerWrapperBase consumer);

        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();


    }

    /// <summary>
    /// Contract for ICommonModel
    /// </summary>
    [ContractClassFor(typeof(ICommonModel))]
    public abstract class CommonModelContract : ICommonModel
    {
        /// <summary>
        /// Creates the basic properties.
        /// </summary>
        /// <returns></returns>
        public ICommonModelProperties CreateBasicProperties()
        {
            Contract.Ensures(Contract.Result<ICommonModelProperties>() != null);

            return default(ICommonModelProperties);
        }

        /// <summary>
        /// Enable publisher acknowledgements.
        /// </summary>
        public void ConfirmSelect()
        {
        }

        /// <summary>
        /// Exchanges the declare.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        public void ExchangeDeclare(string exchangeName, string exchangeType)
        {
            Contract.Requires<ArgumentNullException>(exchangeName != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(exchangeType));

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
        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, ICommonModelProperties properties, byte[] body)
        {
            Contract.Requires<ArgumentNullException>(exchangeName != null);
            Contract.Requires<ArgumentNullException>(properties != null);
            Contract.Requires<ArgumentNullException>(body != null);
        }

        /// <summary>
        /// Wait until all published messages have been confirmed.
        /// </summary>
        /// <param name="confirmationTimeout">The confirmation timeout.</param>
        public void WaitForConfirmsOrDie(TimeSpan confirmationTimeout)
        {
            Contract.Requires<ArgumentNullException>(confirmationTimeout != null);
        }

        /// <summary>
        /// Basic quality of service.
        /// </summary>
        /// <param name="prefetchSize">Size of the prefetch.</param>
        /// <param name="prefetchCount">The prefetch count.</param>
        /// <param name="global">if set to <c>true</c> global.</param>
        public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
        }

        /// <summary>
        /// Gets or sets the model shutdown.
        /// </summary>
        /// <value>
        /// The model shutdown.
        /// </value>
        public EventHandler<ModelShutdownEventArgs> ModelShutdown { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>b
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Basics the ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        public void BasicAck(DeliveryTagWrapper deliveryTag, string exchange, string routingKey, bool multiple)
        {
            Contract.Requires<ArgumentNullException>(deliveryTag != null);
            Contract.Requires<ArgumentNullException>(exchange != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(routingKey));
        }

        /// <summary>
        /// Basics the nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        public void BasicNack(DeliveryTagWrapper deliveryTag, string exchange, string routingKey, bool multiple, bool requeue)
        {
            Contract.Requires<ArgumentNullException>(deliveryTag != null);
            Contract.Requires<ArgumentNullException>(exchange != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(routingKey));
        }

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <returns></returns>
        public ICommonQueue QueueDeclare()
        {
            Contract.Ensures(Contract.Result<ICommonQueue>() != null);

            return default(ICommonQueue);
        }

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="durable">if set to <c>true</c> durable.</param>
        /// <param name="exclusive">if set to <c>true</c> exclusive.</param>
        /// <param name="autoDelete">if set to <c>true</c> auto delete.</param>
        /// <param name="arguments">The arguments.</param>
        public ICommonQueue QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));

            return default(ICommonQueue);
        }

        /// <summary>
        /// Queues the bind.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
            Contract.Requires<ArgumentNullException>(exchangeName != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(routingKey));
        }

        /// <summary>
        /// Creates the subscription to the model.
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public ISubscription CreateSubscription(string queueName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));

            Contract.Ensures(Contract.Result<ISubscription>() != null);

            return default(ISubscription);
        }

        /// <summary>
        /// Basics the consume.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="consumer">The consumer.</param>
        public void BasicConsume(CancellationToken token, string queueName, bool noAck, IConsumerWrapperBase consumer)
        {
            Contract.Requires<ArgumentNullException>(token != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
            Contract.Requires<ArgumentNullException>(consumer != null);
        }


        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue { get; private set; }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}