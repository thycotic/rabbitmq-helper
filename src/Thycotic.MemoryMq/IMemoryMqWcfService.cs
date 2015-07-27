using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using Thycotic.Wcf;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Interface for a memory mq server
    /// </summary>
    [ContractClass(typeof(MemoryMqWcfServiceContract))]
    [ServiceContract(Namespace = "http://www.thycotic.com/services", SessionMode = SessionMode.Required, CallbackContract = typeof(IMemoryMqWcfServiceCallback))]
    public interface IMemoryMqWcfService : IWcfService
    {
        /// <summary>
        /// Basic publish.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="immediate">if set to <c>true</c> [immediate].</param>
        /// <param name="properties"></param>
        /// <param name="body">The body.</param>
        [OperationContract(IsOneWay = true)]
        void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, MemoryMqProperties properties, byte[] body);

        /// <summary>
        /// Binds a queue to an exchange and a routing key
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        [OperationContract(IsOneWay = true)]
        void QueueBind(string queueName, string exchangeName, string routingKey);

        /// <summary>
        /// Basic consume
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        [OperationContract(IsOneWay = false)]
        void BasicConsume(string queueName);

        /// <summary>
        /// Basic ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        [OperationContract(IsOneWay = true)]
        void BasicAck(ulong deliveryTag, string exchange, string routingKey, bool multiple);

        /// <summary>
        /// Basic nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        [OperationContract(IsOneWay = true)]
        void BasicNack(ulong deliveryTag, string exchange, string routingKey, bool multiple, bool requeue);

        /// <summary>
        /// Returns the version of the MemoryMQ Server
        /// </summary>
        /// <returns>The version of the MemoryMQ Server</returns>
        [OperationContract(IsOneWay = false)]
        string GetServerVersion();


    }

    /// <summary>
    /// Contract for IMemoryMqWcfService
    /// </summary>
    [ContractClassFor(typeof(IMemoryMqWcfService))]
    public abstract class MemoryMqWcfServiceContract : IMemoryMqWcfService
    {
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
            Contract.Requires<ArgumentNullException>(exchangeName !=null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(routingKey));
            Contract.Requires<ArgumentNullException>(properties != null);
            Contract.Requires<ArgumentNullException>(body != null);
        }

        /// <summary>
        /// Binds a queue to an exchange and a routing key
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
            Contract.Requires<ArgumentNullException>(exchangeName != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(routingKey));
        }

        /// <summary>
        /// Basic consume
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void BasicConsume(string queueName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
        }

        /// <summary>
        /// Basic ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        public void BasicAck(ulong deliveryTag, string exchange, string routingKey, bool multiple)
        {
            Contract.Requires<ArgumentNullException>(exchange != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(routingKey));
        }

        /// <summary>
        /// Basic nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        public void BasicNack(ulong deliveryTag, string exchange, string routingKey, bool multiple, bool requeue)
        {
            Contract.Requires<ArgumentNullException>(exchange != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(routingKey));
        }

        /// <summary>
        /// Returns the version of the MemoryMQ Server
        /// </summary>
        /// <returns>The version of the MemoryMQ Server</returns>
        public string GetServerVersion()
        {
            return null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}