namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq queue
    /// </summary>
    public class MemoryMqQueue : ICommonQueue
    {
        private readonly string _queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqQueue"/> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public MemoryMqQueue(string queueName)
        {
            _queueName = queueName;
        }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get { return _queueName; } }
    }
}