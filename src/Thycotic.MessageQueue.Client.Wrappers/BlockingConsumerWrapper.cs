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
    /// RPC consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public class BlockingConsumerWrapper<TRequest, TResponse, THandler> : ConsumerWrapperBase<TRequest, THandler>
        where TRequest : class, IBlockingConsumable
        where TResponse : class
        where THandler : IBlockingConsumer<TRequest, TResponse>
    {
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMessageEncryptor _messageEncryptor;
        private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly ICommonConnection _connection;
        private readonly ILogWriter _log = Log.Get(typeof(BlockingConsumerWrapper<TRequest, TResponse, THandler>));

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockingConsumerWrapper{TRequest,TResponse,THandler}" /> class.
        /// </summary>
        /// <param name="connection">The RMQ.</param>
        /// <param name="exchangeNameProvider">The exchange provider.</param>
        /// <param name="objectSerializer">The serializer.</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        public BlockingConsumerWrapper(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<THandler>> handlerFactory)
            : base(connection, exchangeNameProvider)
        {

            _objectSerializer = objectSerializer;
            _messageEncryptor = messageEncryptor;
            _handlerFactory = handlerFactory;
            _connection = connection;

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
            Task.Run(() => ExecuteMessage(deliveryTag, exchange, routingKey, properties, body));
        }

        /// <summary>
        /// Executes the message.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchangeName">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        private void ExecuteMessage(ulong deliveryTag, string exchangeName, string routingKey, ICommonModelProperties properties, byte[] body)
        {
            var responseType = BlockingConsumerResponseTypes.Success;
            object response;

            try
            {

                TRequest message;

                try
                {
                    message = _objectSerializer.ToObject<TRequest>(_messageEncryptor.Decrypt(exchangeName, body));

                }
                catch (Exception ex)
                {
                    _log.Error("Failed to decrypt or deserialize message", ex);

                    throw;
                }

                using (var handler = _handlerFactory())
                {
                    response = handler.Value.Consume(message);

                    _log.Debug(string.Format("Successfully processed {0}", this.GetRoutingKey(typeof(TRequest))));
                }

                CommonModel.BasicAck(deliveryTag, exchangeName, routingKey, false);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Failed to process {0} because {1}", this.GetRoutingKey(typeof(TRequest)), ex.Message), ex);

                CommonModel.BasicNack(deliveryTag, exchangeName, routingKey, false, requeue: false);

                response = new BlockingConsumerError { Message = ex.Message };
                responseType = BlockingConsumerResponseTypes.Error;
            }

            if (properties.IsReplyToPresent())
            {
                Respond(exchangeName, properties.ReplyTo, response, properties.CorrelationId, responseType);
            }
        }

        private void Respond(string originatingExchangeName, string replyTo, object response, string correlationId, string type)
        {
            try
            {
                var routingKey = replyTo;

                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    channel.ConfirmSelect();

                    var properties = channel.CreateBasicProperties();

                    properties.CorrelationId = correlationId;
                    properties.Type = type;

                    //reply-to's do not use exchange names since there is a reply-to address
                    var replyToExchangeName = string.Empty;

                    channel.BasicPublish(replyToExchangeName, routingKey, DefaultConfigValues.Model.Publish.NotMandatory,
                        DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties,
                        _messageEncryptor.Encrypt(originatingExchangeName, _objectSerializer.ToBytes(response)));

                    channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);
                }

            }
            catch (Exception ex)
            {
                _log.Error("Failed to respond to caller", ex);
            }
        }
    }
}