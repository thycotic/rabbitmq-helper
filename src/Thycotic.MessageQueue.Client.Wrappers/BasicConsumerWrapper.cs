using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Basic consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public class BasicConsumerWrapper<TRequest, THandler> : ConsumerWrapperBase<TRequest, THandler>
        where TRequest : class, IConsumable
        where THandler : IBasicConsumer<TRequest>
    {
        private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMessageEncryptor _messageEncryptor;
        private readonly ILogWriter _log = Log.Get(typeof(BasicConsumerWrapper<TRequest, THandler>));

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicConsumerWrapper{TRequest,THandler}" /> class.
        /// </summary>
        /// <param name="connection">The RMQ.</param>
        /// <param name="exchangeNameProvider">The exchange provider.</param>
        /// <param name="objectSerializer">The serializer.</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        public BasicConsumerWrapper(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<THandler>> handlerFactory)
            : base(connection, exchangeNameProvider)
        {
            _handlerFactory = handlerFactory;
            _objectSerializer = objectSerializer;
            _messageEncryptor = messageEncryptor;
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
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey, ICommonModelProperties properties, byte[] body)
        {
            Task.Run(() => ExecuteMessage(deliveryTag, exchange, routingKey, body));
        }

        /// <summary>
        /// Executes the message.
        /// </summary>
        /// <param name="exchangeName">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="body">The body.</param>
        private void ExecuteMessage(ulong deliveryTag, string exchangeName, string routingKey, byte[] body)
        {
            const bool multiple = false;

            using (LogContext.Create("Processing message..."))
            {
                try
                {
                    var message = _objectSerializer.ToObject<TRequest>(_messageEncryptor.Decrypt(exchangeName, body));

                    using (var handler = _handlerFactory())
                    {
                        handler.Value.Consume(message);
                    }

                    _log.Debug(string.Format("Successfully processed {0}", this.GetRoutingKey(typeof(TRequest))));

                    CommonModel.BasicAck(deliveryTag, exchangeName, routingKey, multiple);

                }
                catch (Exception e)
                {
                    _log.Error(string.Format("Failed to process {0}", this.GetRoutingKey(typeof(TRequest))), e);

                    CommonModel.BasicNack(deliveryTag, exchangeName, routingKey, multiple, requeue: true);
                }
            }
        }
    }
}
