using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Thycotic.MessageQueue.Client.QueueClient;

namespace Thycotic.MessageQueue.Client.Wrappers.Proxies
{
    /// <summary>
    /// Common wrapper proxy
    /// </summary>
    public class CommonConsumerWrapperProxy : IConsumerWrapperBase
    {
        /// <summary>
        /// The target of the proxy.
        /// </summary>
        protected  IConsumerWrapperBase Target { get; private set; }

        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        public ICommonModel CommonModel
        {
            get { return Target.CommonModel; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonConsumerWrapperProxy" /> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public CommonConsumerWrapperProxy(IConsumerWrapperBase target)
        {
            Contract.Requires<ArgumentNullException>(target != null);
            
            Target = target;
        }

        /// <summary>
        /// Starts the consuming.
        /// </summary>
        public void StartConsuming()
        {
            Target.StartConsuming();
        }

        /// <summary>
        /// Called each time a message arrives for this consumer.
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <param name="deliveryTag"></param>
        /// <param name="redelivered"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="properties"></param>
        /// <param name="body"></param>
        /// <remarks>
        /// Be aware that acknowledgement may be required. See IModel.BasicAck.
        /// </remarks>
        public Task HandleBasicDeliver(string consumerTag, DeliveryTagWrapper deliveryTag, bool redelivered, string exchange, string routingKey, ICommonModelProperties properties,
            byte[] body)
        {
            var task = Target.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);

            return task;
        }

        /// <summary>
        /// Sets Thread Priority for proxied Wrapper
        /// </summary>
        /// <param name="scheduler"></param>
        public void SetPriority(IPriorityScheduler scheduler)
        {
            Target.SetPriority(scheduler);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Target.Dispose();
        }

    }
}