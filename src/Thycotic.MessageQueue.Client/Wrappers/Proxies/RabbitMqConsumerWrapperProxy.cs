using System;
using System.Diagnostics.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.QueueClient.RabbitMq;

namespace Thycotic.MessageQueue.Client.Wrappers.Proxies
{
    /// <summary>
    /// Rabbit Mq consumer wrapper proxy
    /// </summary>
    public class RabbitMqConsumerWrapperProxy : CommonConsumerWrapperProxy, IBasicConsumer
    {
        

        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        public IModel Model { get { return CommonModel.GetRawValue<IModel>(); } }

#pragma warning disable 0067
        /// <summary>
        /// Signaled when the consumer gets cancelled.
        /// </summary>
        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;
#pragma warning restore 0067

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConsumerWrapperProxy" /> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="processCounter">The process counter.</param>
        public RabbitMqConsumerWrapperProxy(IConsumerWrapperBase target, ProcessCounter processCounter) : base(target, processCounter)
        {
            Contract.Requires<ArgumentNullException>(target != null);
            Contract.Requires<ArgumentNullException>(processCounter != null);
        }
        
        #region Not needed
        /// <summary>
        /// Called upon successful registration of the
        /// consumer with the broker.
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicConsumeOk(string consumerTag)
        {
        }

        /// <summary>
        /// Called upon successful deregistration of the
        /// consumer from the broker.
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicCancelOk(string consumerTag)
        {
        }

        /// <summary>
        /// Called when the consumer is cancelled for reasons other than by a
        /// basicCancel: e.g. the queue has been deleted (either by this channel or
        /// by any other channel). See handleCancelOk for notification of consumer
        /// cancellation due to basicCancel.
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicCancel(string consumerTag)
        {
        }

        /// <summary>
        /// Called when the model shuts down.
        /// </summary>
        /// <param name="model">Common AMQP model.</param>
        /// <param name="reason">Information about the reason why a particular model, session, or connection was destroyed.</param>
        public void HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
        }
        #endregion

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
        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties,
            byte[] body)
        {
            base.HandleBasicDeliver(consumerTag, new DeliveryTagWrapper(deliveryTag), redelivered, exchange, routingKey, new RabbitMqModelProperties(properties), body );
        }
    }
}
