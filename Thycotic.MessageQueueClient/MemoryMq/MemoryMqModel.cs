using System;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq model
    /// </summary>
    public class MemoryMqModel : IMemoryMqModel
    {
        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <returns></returns>
        public IMemoryMqQueue QueueDeclare()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the basic properties.
        /// </summary>
        /// <returns></returns>
        public MemoryMqModelProperties CreateBasicProperties()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Confirms the select.
        /// </summary>
        public void ConfirmSelect()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Exchanges the declare.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        public void ExchangeDeclare(string exchangeName, string exchangeType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Basics the publish.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="doNotDeliverImmediatelyOrRequireAListener">if set to <c>true</c> [do not deliver immediately or require a listener].</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool doNotDeliverImmediatelyOrRequireAListener,
            MemoryMqModelProperties properties, byte[] body)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Waits for confirms or die.
        /// </summary>
        /// <param name="confirmationTimeout">The confirmation timeout.</param>
        public void WaitForConfirmsOrDie(TimeSpan confirmationTimeout)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}