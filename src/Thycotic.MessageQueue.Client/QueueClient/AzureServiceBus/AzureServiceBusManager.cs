using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Thycotic.Logging;

namespace Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus
{
    /// <summary>
    /// Azure service bus manager.
    /// </summary>
    public class AzureServiceBusManager : IAzureServiceBusManager
    {
        private static readonly ConcurrentDictionary<string, string> HashValues = new ConcurrentDictionary<string, string>();

        private readonly ILogWriter _log = Log.Get(typeof (AzureServiceBusManager));

        private static NamespaceManager _namespaceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusManager" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sharedAccessKeyName">Name of the shared access key.</param>
        /// <param name="sharedAccessKeyValue">The shared access key value.</param>
        public AzureServiceBusManager(string connectionString, string sharedAccessKeyName, string sharedAccessKeyValue)
        {
            _namespaceManager = GetNamespaceManager(connectionString, sharedAccessKeyName, sharedAccessKeyValue);
        }

        /// <summary>
        /// Creates the topic.
        /// </summary>
        /// <param name="topicName">The name.</param>
        /// <param name="deleteExisting">if set to <c>true</c> [delete previous].</param>
        public void CreateTopic(string topicName, bool deleteExisting = false)
        {
            var description = new TopicDescription(topicName)
            {
                EnableExpress = true,
                EnableFilteringMessagesBeforePublishing = true
            };

            // Delete the topic if it already exists
            if (_namespaceManager.TopicExists(topicName))
            {
                if (deleteExisting)
                {
                    _log.Warn(string.Format("Deleting previous topic {0}", topicName));
                    _namespaceManager.DeleteTopic(topicName);
                }
                else
                {
                    return;
                }
            }

            _log.Debug(string.Format("Creating topic {0}", topicName));
            _namespaceManager.CreateTopic(description);
        }

        /// <summary>
        /// Creates the queue.
        /// </summary>
        /// <param name="queueName">Name of the topic.</param>
        /// <param name="autoDelete">if set to <c>true</c> [automatic delete].</param>
        /// <param name="deleteExisting">if set to <c>true</c> [delete existing].</param>
        public void CreateQueue(string queueName, bool autoDelete = false, bool deleteExisting = false)
        {
            var description = new QueueDescription(queueName)
            {
                EnableExpress = true,
                EnableDeadLetteringOnMessageExpiration = true
            };

            if (autoDelete)
            {
                description.AutoDeleteOnIdle = TimeSpan.FromMinutes(5);
            }

            // Delete the topic if it already exists
            if (_namespaceManager.QueueExists(queueName))
            {
                if (deleteExisting)
                {
                    _log.Warn(string.Format("Deleting previous queue {0}", queueName));
                    _namespaceManager.DeleteQueue(queueName);
                }
                else
                {
                    return;
                }
            }

            _log.Debug(string.Format("Creating queue {0}", queueName));
            _namespaceManager.CreateQueue(description);
        }

        /// <summary>
        /// Creates the routing key subscription.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="sessions">if set to <c>true</c> [sessions].</param>
        /// <param name="deleteExisting">if set to <c>true</c> [delete previous].</param>
        public void CreateRoutingKeyQueueSubscription(string queueName, string topicName, string routingKey, bool sessions = false,
            bool deleteExisting = false)
        {
            CreateSqlStringSubscription(queueName, topicName, AzureServiceBusConnection.SubscriptionNames.RoutingKey, routingKey, sessions, deleteExisting);
        }

        private static string GetHashValue(string input)
        {
            return HashValues.GetOrAdd(input, i =>
            {
                using (var generator = new BasicHashGenerator(SHA1.Create()))
                {
                    var hash = generator.GetHash(i);

                    return hash;
                }
            });

        }


        private void CreateSqlStringSubscription(string queueName, string topicName, string key, string value, bool sessions, bool deletePrevious = false)
        {
            //addresses a 50 character limitation for subscription names in azure
            var subscriptionName = GetHashValue(queueName);

            var description = new SubscriptionDescription(topicName, subscriptionName)
            {
                EnableDeadLetteringOnMessageExpiration = true,
                EnableDeadLetteringOnFilterEvaluationExceptions = true,
                RequiresSession = sessions,
                ForwardTo = queueName
            };

            // Delete the subscription if it already exists
            if (_namespaceManager.SubscriptionExists(description.TopicPath, description.Name))
            {
                if (deletePrevious)
                {
                    _log.Warn(string.Format("Deleting previous subscription with name {0} under {1}", description.Name, description.TopicPath));
                    _namespaceManager.DeleteSubscription(description.TopicPath, description.Name);
                }
                else
                {
                    return;
                }
            }

            _log.Debug(string.Format("Creating subscription for {0} (sessions: {1})", description.Name, sessions));

            var filter = new SqlFilter(string.Format(@"[{0}]='{1}'", AzureBrokedMessageExtensions.GetCustomePropertyKey(key), value));
            filter.Validate();

            _namespaceManager.CreateSubscription(description, filter);
        }

        private static NamespaceManager GetNamespaceManager(string connectionString, string sharedAccessKeyName, string sharedAccessKeyValue)
        {
            var uri = new Uri(connectionString);
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sharedAccessKeyName, sharedAccessKeyValue);

            return new NamespaceManager(uri, tokenProvider);
        }

        private class BasicHashGenerator : IDisposable
        {
            private bool _disposed;
            private readonly HashAlgorithm _algorithm;

            /// <summary>
            /// Initializes a new instance of the <see cref="BasicHashGenerator"/> class.
            /// </summary>
            /// <param name="algorithm">The algorithm.</param>
            public BasicHashGenerator(HashAlgorithm algorithm)
            {
                _algorithm = algorithm;
            }

            /// <summary>
            /// Gets the hash of some source.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <returns></returns>
            public string GetHash(string source)
            {

                var hash = GenerateHash(source);

                //Console.WriteLine("The MD5 hash of " + source + " is: " + hash + ".");

                //Console.WriteLine("Verifying the hash...");

                VerifyMd5Hash(source, hash);

                return hash;


            }

            private string GenerateHash(string input)
            {

                // Convert the input string to a byte array and compute the hash.
                var data = _algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                foreach (var t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

            private void VerifyMd5Hash(string input, string hash)
            {
                // Hash the input.
                var hashOfInput = GenerateHash(input);

                // Create a StringComparer an compare the hashes.
                var comparer = StringComparer.OrdinalIgnoreCase;

                if (0 != comparer.Compare(hashOfInput, hash))
                {
                    throw new Exception("Hash validation failed");
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }

                _algorithm.Dispose();

                _disposed = true;
            }
        }
    }
}
