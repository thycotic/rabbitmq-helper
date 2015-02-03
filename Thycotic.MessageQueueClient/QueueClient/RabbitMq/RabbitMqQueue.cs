using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.QueueClient.RabbitMq
{
    internal class RabbitMqQueue : ICommonQueue
    {
        private readonly QueueDeclareOk _rawQueue;

        public RabbitMqQueue(QueueDeclareOk rawQueue)
        {
            _rawQueue = rawQueue;
        }

        public object QueueName {
            get { return _rawQueue.QueueName; }
        }
    }
}