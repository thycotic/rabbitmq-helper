using System;
using RabbitMQ.Client;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    /// <summary>
    /// Routing helpers
    /// </summary>
    public static class RoutingHelpers
    {
        /// <summary>
        /// Gets the routing key based on the current consumable.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string GetRoutingKey(this IConsumable obj)
        {
            return obj.GetType().FullName;
        }

        /// <summary>
        /// Gets the routing key based on the specified consumable.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="consumableType">Type of the consumable.</param>
        /// <returns></returns>
        public static string GetRoutingKey(this IBasicConsumer consumer, Type consumableType)
        {
            return consumableType.FullName;
        }

        /// <summary>
        /// Gets the name of the queue based on the specified consumer and consumable.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="consumerType">Type of the consumer.</param>
        /// <param name="consumableType">Type of the consumable.</param>
        /// <returns></returns>
        public static string GetQueueName(this IBasicConsumer consumer, Type consumerType, Type consumableType)
        {
            return string.Format("{0}:{1}", consumerType.FullName, consumer.GetRoutingKey(consumableType));
        }

    }
}
