using RabbitMQ.Client;

namespace Thycotic.MessageQueue.Client.QueueClient.RabbitMq
{
    internal class RabbitMqQueue : ICommonQueue
    {
        private readonly QueueDeclareOk _rawQueue;

        public RabbitMqQueue(QueueDeclareOk rawQueue)
        {
            _rawQueue = rawQueue;
        }

        public string QueueName {
            get { return _rawQueue.QueueName; }
        }
    }
}