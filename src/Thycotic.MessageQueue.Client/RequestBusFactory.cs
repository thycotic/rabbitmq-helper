using System;
using System.Diagnostics.Contracts;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
using Thycotic.MessageQueue.Client.QueueClient.RabbitMq;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Request bus factory
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
        /// <param name="connection">The connection for the bus which should be disposed after the bus is no longer needed.</param>
        /// <returns></returns>
        public static IRequestBus GetRabbitMqBus(string url, string userName, string password, bool useSsl, IMessageEncryptor messageEncryptor, out ICommonConnection connection)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(url));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(password));
            Contract.Requires<ArgumentNullException>(messageEncryptor != null);

            Contract.Ensures(Contract.Result<IRequestBus>() != null);
            
            connection = new RabbitMqConnection(url, userName, password, useSsl);

            var objectSerializer = new JsonObjectSerializer();

            return new RequestBus(connection, objectSerializer, messageEncryptor);
        }

        /// <summary>
        /// Gets the Memory mq bus.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <param name="connection">The connection for the bus which should be disposed after the bus is no longer needed.</param>
        /// <returns></returns>
        public static IRequestBus GetMemoryMqBus(string url, string userName, string password, bool useSsl, IMessageEncryptor messageEncryptor, out ICommonConnection connection)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(url));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(password));
            Contract.Requires<ArgumentNullException>(messageEncryptor != null);

            Contract.Ensures(Contract.Result<IRequestBus>() != null);

            connection = new MemoryMqConnection(url, userName, password, useSsl);

            var objectSerializer = new JsonObjectSerializer();

            return new RequestBus(connection, objectSerializer, messageEncryptor);
        }

        /// <summary>
        /// Gets the Azure Service bus.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="sharedAccessKeyName">Name of the shared access key.</param>
        /// <param name="sharedAccessKeyValue">The shared access key value.</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <param name="connection">The connection for the bus which should be disposed after the bus is no longer needed.</param>
        /// <returns></returns>
        public static IRequestBus GetAzureServiceBus(string url, string sharedAccessKeyName, string sharedAccessKeyValue, IMessageEncryptor messageEncryptor, out ICommonConnection connection)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(url));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(sharedAccessKeyName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(sharedAccessKeyValue));
            Contract.Requires<ArgumentNullException>(messageEncryptor != null);

            Contract.Ensures(Contract.Result<IRequestBus>() != null);

            connection = new AzureServiceBusConnection(url, sharedAccessKeyName, sharedAccessKeyValue);

            var objectSerializer = new JsonObjectSerializer();

            return new RequestBus(connection, objectSerializer, messageEncryptor);
        }
    }
}
