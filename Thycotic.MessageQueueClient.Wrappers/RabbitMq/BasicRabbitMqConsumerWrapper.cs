using System;
using Autofac.Features.OwnedInstances;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers.RabbitMq
{
    /// <summary>
    /// Basic Rabbit Mq consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public class BasicRabbitMqConsumerWrapper<TRequest, THandler> : BasicConsumerWrapper<TRequest, THandler>, IBasicConsumer
        where TRequest : IConsumable
        where THandler : IBasicConsumer<TRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRabbitMqConsumerWrapper{TRequest, THandler}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        public BasicRabbitMqConsumerWrapper(ICommonConnection connection, IMessageSerializer serializer, Func<Owned<THandler>> handlerFactory)
            : base(connection, serializer, handlerFactory)
        {
        }

        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public IModel Model { get { return CommonModel.GetRawValue<IModel>(); } }

#pragma warning disable 0067
        /// <summary>
        /// Signalled when the consumer gets cancelled.
        /// </summary>
        public event ConsumerCancelledEventHandler ConsumerCancelled;
#pragma warning restore 0067

        #region Not needed
        /// <summary>
        /// Called upon successful registration of the
        /// consumer with the broker.
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void HandleBasicConsumeOk(string consumerTag)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called upon successful deregistration of the
        /// consumer from the broker.
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void HandleBasicCancelOk(string consumerTag)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when the consumer is cancelled for reasons other than by a
        /// basicCancel: e.g. the queue has been deleted (either by this channel or
        /// by any other channel). See handleCancelOk for notification of consumer
        /// cancellation due to basicCancel.
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void HandleBasicCancel(string consumerTag)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when the model shuts down.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="reason"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            throw new NotImplementedException();
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
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// Be aware that acknowledgement may be required. See IModel.BasicAck.
        /// </remarks>
        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties,
            byte[] body)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
