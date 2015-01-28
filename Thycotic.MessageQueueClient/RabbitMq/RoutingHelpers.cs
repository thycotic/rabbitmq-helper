namespace Thycotic.MessageQueueClient.RabbitMq
{
    public static class RoutingHelpers
    {
        public static string GetRoutingKey(this IConsumable obj)
        {
            return obj.GetType().FullName;
        }

        public static string GetQueueName(this IConsumer consumer, IConsumable consumable)
        {
            return string.Format("{0}:{1}", consumer.GetType().FullName, consumable.GetRoutingKey());
        }

    }
}
