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
        /// Creates the sender.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        MessageSender CreateSender(string entityName);

        /// <summary>
        /// Creates the receiver.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        MessageReceiver CreateReceiver(string entityName);

    }
}