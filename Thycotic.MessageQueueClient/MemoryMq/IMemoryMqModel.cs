using System;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Interface for a Memory Mq model
    /// </summary>
    public interface IMemoryMqModel : IDisposable
    {
        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <returns></returns>
        IMemoryMqQueue QueueDeclare();

        /// <summary>
        /// Creates the basic properties.
        /// </summary>
        /// <returns></returns>
        MemoryMqModelProperties CreateBasicProperties();

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
        void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool doNotDeliverImmediatelyOrRequireAListener, MemoryMqModelProperties properties, byte[] body);

        /// <summary>
        /// Waits for confirms or die.
        /// </summary>
        /// <param name="confirmationTimeout">The confirmation timeout.</param>
        void WaitForConfirmsOrDie(TimeSpan confirmationTimeout);
    }
}