namespace Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus
{
    /// <summary>
    /// Interface for an Azure service bus manager.
    /// </summary>
    public interface IAzureServiceBusManager
    {

        /// <summary>
        /// Creates the topic.
        /// </summary>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="deleteExisting">if set to <c>true</c> [delete previous].</param>
        void CreateTopic(string topicName, bool deleteExisting = false);

        /// <summary>
        /// Creates the queue.
        /// </summary>
        /// <param name="queueName">Name of the topic.</param>
        /// <param name="autoDelete">if set to <c>true</c> [automatic delete].</param>
        /// <param name="deleteExisting">if set to <c>true</c> [delete existing].</param>
        void CreateQueue(string queueName, bool autoDelete = false, bool deleteExisting = false);

        /// <summary>
        /// Creates the routing key subscription.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="sessions">if set to <c>true</c> [sessions].</param>
        /// <param name="deleteExisting">if set to <c>true</c> [delete previous].</param>
        void CreateRoutingKeyQueueSubscription(string queueName, string topicName, string routingKey, bool sessions = false,
            bool deleteExisting = false);
    }
}