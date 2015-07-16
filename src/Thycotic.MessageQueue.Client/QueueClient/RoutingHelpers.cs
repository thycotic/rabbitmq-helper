using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.MessageQueue.Client.Wrappers;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.QueueClient
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
            Contract.Requires<ArgumentNullException>(obj != null);

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var supplement = obj.GetType().GetInterfaces().Contains(typeof(IBlockingConsumable)) ? "Blocking" : "Basic";
            return string.Format("{0}:{1}", obj.GetType().FullName, supplement);
        }

        /// <summary>
        /// Gets the routing key based on the specified consumable.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="consumableType">Type of the consumable.</param>
        /// <returns></returns>
        public static string GetRoutingKey(this IConsumerWrapperBase consumer, Type consumableType)
        {
            Contract.Requires<ArgumentNullException>(consumer != null);
            Contract.Requires<ArgumentNullException>(consumableType != null);

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var supplement = consumableType.GetInterfaces().Contains(typeof(IBlockingConsumable)) ? "Blocking" : "Basic";
            return string.Format("{0}:{1}", consumableType.FullName, supplement);
        }

        /// <summary>
        /// Gets the name of the queue based on the specified consumer and consumable.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="consumerType">Type of the consumer.</param>
        /// <param name="consumableType">Type of the consumable.</param>
        /// <returns></returns>
        public static string GetQueueName(this IConsumerWrapperBase consumer, string exchangeName, Type consumerType, Type consumableType)
        {
            Contract.Requires<ArgumentNullException>(consumer != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(exchangeName));
            Contract.Requires<ArgumentNullException>(consumerType != null);
            Contract.Requires<ArgumentNullException>(consumableType != null);

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var queueName = string.Format("{0}:{1}:{2}", exchangeName, consumerType.FullName, consumer.GetRoutingKey(consumableType));

            Contract.Assume(queueName != null);
            Contract.Assume(!string.IsNullOrWhiteSpace(queueName));

            return queueName;
        }

    }
}
