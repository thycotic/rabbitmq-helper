namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory message queue model
    /// </summary>
    public class MemoryMqModel : IMemoryMqModel
    {
        private readonly IMemoryMqConnection _memoryMqConnection;
        private MemoryMqBinding _binding;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqModel"/> class.
        /// </summary>
        /// <param name="memoryMqConnection">The memory mq connection.</param>
        public MemoryMqModel(IMemoryMqConnection memoryMqConnection)
        {
            _memoryMqConnection = memoryMqConnection;
        }

        /// <summary>
        /// Declares the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void QueueDeclare(string queueName)
        {
            _memoryMqConnection.CreateQueue(queueName);
        }

        /// <summary>
        /// Binds to the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void QueueBind(string queueName)
        {
            _binding = new MemoryMqBinding(_memoryMqConnection.GetQueue(queueName));
        }

        /// <summary>
        /// Begins the consumption process.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="consumer">The consumer.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void BasicConsume(string queueName, bool noAck, IConsumerWrapperBase consumer)
        {
            _binding.BeginScanning(consumer);
        }
    }
}