using System;
using System.Diagnostics.Contracts;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
using Thycotic.MessageQueue.Client.QueueClient.RabbitMq;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Client
{
    /// <summary>
    /// Basic request bus factory
    /// </summary>
    public static class RequestBusFactory
    {
        /// <summary>
        /// Gets the Rabbit mq bus.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <returns></returns>
        public static IRequestBus GetRabbitMqBus(string url, string userName, string password, bool useSsl, IMessageEncryptor messageEncryptor)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(url));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(password));
            Contract.Requires<ArgumentNullException>(messageEncryptor != null);

            Contract.Ensures(Contract.Result<IRequestBus>() != null);
            
            var connection = new RabbitMqConnection(url, userName, password, useSsl);

            var objectSerializer = new JsonObjectSerializer();

            

            return new RequestBus(connection, objectSerializer, messageEncryptor);
        }

        /// <summary>
        /// Gets the memory mq bus.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <returns></returns>
        public static IRequestBus GetMemoryMqBus(string url, string userName, string password, bool useSsl, IMessageEncryptor messageEncryptor)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(url));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(password));
            Contract.Requires<ArgumentNullException>(messageEncryptor != null);

            Contract.Ensures(Contract.Result<IRequestBus>() != null);

            var connection = new MemoryMqConnection(url, userName, password, useSsl);

            var objectSerializer = new JsonObjectSerializer();

            return new RequestBus(connection, objectSerializer, messageEncryptor);
        }
    }
}
