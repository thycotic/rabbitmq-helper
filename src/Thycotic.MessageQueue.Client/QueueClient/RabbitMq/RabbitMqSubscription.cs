using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Thycotic.MessageQueue.Client.Wrappers;

namespace Thycotic.MessageQueue.Client.QueueClient.RabbitMq
{
    internal class RabbitMqSubscription : ISubscription
    {
        private readonly RabbitMqModel _model;
        private readonly Subscription _subscription;

        public RabbitMqSubscription(RabbitMqModel model, string queueName)
        {
            _model = model;
            _subscription = new Subscription(model.GetRawValue<IModel>(), queueName);
        }

        public void Dispose()
        {
        }

        public string QueueName {
            get { return _subscription.QueueName; }
        }
        public bool Next(int timeoutMilliseconds, out CommonDeliveryEventArgs response)
        {
            response = null;

            BasicDeliverEventArgs eventArgs;
            if (!_subscription.Next(timeoutMilliseconds, out eventArgs))
            {
                return false;
            }

            _model.BasicAck(new DeliveryTagWrapper(eventArgs.DeliveryTag), eventArgs.Exchange, eventArgs.RoutingKey, false);

            response = new CommonDeliveryEventArgs(eventArgs.ConsumerTag, new DeliveryTagWrapper(eventArgs.DeliveryTag), eventArgs.Redelivered, eventArgs.Exchange,
                eventArgs.RoutingKey, new RabbitMqModelProperties(eventArgs.BasicProperties), eventArgs.Body);
            return true;
        }
    }
}