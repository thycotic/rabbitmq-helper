using System;
using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public static class RoutingHelpers
    {
        public static string GetRoutingKey(this IConsumable obj)
        {
            return obj.GetType().FullName;
        }

        public static string GetRoutingKey(this IBasicConsumer consumer, Type consuableType)
        {
            return consuableType.FullName;
        }

        public static string GetQueueName(this IBasicConsumer consumer, Type consumerType, Type consumableType)
        {
            return string.Format("{0}:{1}", consumerType.FullName, consumer.GetRoutingKey(consumableType));
        }

    }
}
