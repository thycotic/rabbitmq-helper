using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public class SimpleConsumerWrapper<TRequest, THandler> : ConsumerWrapperBase<TRequest, THandler>
    {
        public SimpleConsumerWrapper(IRabbitMqConnection connection) : base(connection)
        {
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            
        }
    }
}
