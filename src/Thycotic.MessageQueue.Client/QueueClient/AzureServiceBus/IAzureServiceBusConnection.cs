using System;
using Microsoft.ServiceBus.Messaging;
using Thycotic.Utility.TestChain;

namespace Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus
{
    /// <summary>
    /// Interface for an Azure service bus connection
    /// </summary>
    [UnitTestsRequired]
    public interface IAzureServiceBusConnection : ICommonConnection
    {
        /// <summary>
        /// Creates the manager.
        /// </summary>
        /// <returns></returns>
        IAzureServiceBusManager CreateManager();

        /// <summary>
        /// Creates the topic client.
        /// </summary>
        /// <param name="topicPath">The topic path.</param>
        /// <returns></returns>
        TopicClient CreateTopicClient(string topicPath);

        /// <summary>
        /// Creates the queue client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        MessageReceiver CreateQueueClient(string queueName);

        ///// <summary>
        ///// Creates the subscription client.
        ///// </summary>
        ///// <param name="topicPath">The topic path.</param>
        ///// <param name="subscriptionName">Name of the subscription.</param>
        ///// <returns></returns>
        //SubscriptionClient CreateSubscriptionClient(string topicPath, string subscriptionName);

        
    }
}