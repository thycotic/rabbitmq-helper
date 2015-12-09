using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus
{
    /// <summary>
    /// Azure service bus connection
    /// </summary>
    public class AzureServiceBusConnection : IAzureServiceBusConnection
    {
        /// <summary>
        /// Subscription names
        /// </summary>
        public static class SubscriptionNames
        {
            /// <summary>
            /// The routing key
            /// </summary>
            public const string RoutingKey = "RoutingKey";
        }

        private static readonly Regex ConnectionStringRegex = new Regex(@"([\w]+)=([^;]+);?", RegexOptions.Compiled );

        private readonly string _connectionString;

        /// <summary>
        /// Server version
        /// </summary>
        public string ServerVersion {
            get
            {   
                //TODO: flesh out the version
                return "";
            }
        }

        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        public EventHandler ConnectionCreated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusConnection"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AzureServiceBusConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Creates the manager.
        /// </summary>
        /// <returns></returns>
        public IAzureServiceBusManager CreateManager()
        {
            return new AzureServiceBusManager(_connectionString);
        }

        /// <summary>
        /// Creates the topic client.
        /// </summary>
        /// <param name="topicPath">The topic path.</param>
        /// <returns></returns>
        public TopicClient CreateTopicClient(string topicPath)
        {
            var messagingFactory = GetFactory(_connectionString);

            return messagingFactory.CreateTopicClient(topicPath);
        }

        /// <summary>
        /// Creates the message sender.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        public MessageSender CreateSender(string entityName)
        {
            var messagingFactory = GetFactory(_connectionString);

            return messagingFactory.CreateMessageSender(entityName);
        }
        
        /// <summary>
        /// Creates the queue client.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        public MessageReceiver CreateReceiver(string entityName)
        {
            var messagingFactory = GetFactory(_connectionString);

            return messagingFactory.CreateMessageReceiver(entityName, ReceiveMode.PeekLock);
        }
       
        private static MessagingFactory GetFactory(string connectionString)
        {
            var match = ConnectionStringRegex.Matches(connectionString);

            if (match.Count != 3)
            {
                throw new ConfigurationErrorsException("Connection string could not be parsed");
            }

            //key = group 1
            //value = group 2
            var values = Enumerable.Range(0, match.Count).ToDictionary(i => match[i].Groups[1].Value, i => match[i].Groups[2].Value);

            var serviceBusFqdn = values["Endpoint"];
            var serviceBusKeyName = values["SharedAccessKeyName"];
            var serviceBusKey = values["SharedAccessKey"];

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(serviceBusKeyName, serviceBusKey);

            var uri = new Uri(serviceBusFqdn);
            return MessagingFactory.Create(uri, tokenProvider);
        }

        /// <summary>
        /// Opens the model/channel.
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        public ICommonModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor)
        {
            return new AzureServiceBusModel(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //nothing to dispose
        }

        
    }
}