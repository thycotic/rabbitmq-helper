using Thycotic.MessageQueueClient.RabbitMq;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Interface for a memory message queue model
    /// </summary>
    public interface IMemoryMqModel
    {
        /// <summary>
        /// Declares the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        void QueueDeclare(string queueName);

        /// <summary>
        /// Binds to the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        void QueueBind(string queueName);

        /// <summary>
        /// Begins the consumption process.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="consumer">The consumer.</param>
        void BasicConsume(string queueName, bool noAck, IConsumerWrapperBase consumer);
    }
}