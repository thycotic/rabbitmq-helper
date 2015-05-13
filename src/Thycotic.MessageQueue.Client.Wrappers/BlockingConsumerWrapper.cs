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
    /// <typeparam name="TConsumable">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="TConsumer">The type of the handler.</typeparam>
    public class BlockingConsumerWrapper<TConsumable, TResponse, TConsumer> : ConsumerWrapperBase<TConsumable, TConsumer>
        where TConsumable : class, IBlockingConsumable
        where TResponse : class
        where TConsumer : IBlockingConsumer<TConsumable, TResponse>
    {
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMessageEncryptor _messageEncryptor;
        private readonly Func<Owned<TConsumer>> _consumerFactory;
        private readonly ICommonConnection _connection;
        private readonly ILogWriter _log = Log.Get(typeof(BlockingConsumerWrapper<TConsumable, TResponse, TConsumer>));


        /// <summary>
        /// Initializes a new instance of the <see cref="BlockingConsumerWrapper{TConsumable, TResponse, TConsumer}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="exchangeNameProvider">The exchange name provider.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <param name="consumerFactory">The handler factory.</param>
        public BlockingConsumerWrapper(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<TConsumer>> consumerFactory)
            : base(connection, exchangeNameProvider)
        {

            _objectSerializer = objectSerializer;
            _messageEncryptor = messageEncryptor;
            _consumerFactory = consumerFactory;
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

                TConsumable message;

                try
                {
                    message = _objectSerializer.ToObject<TConsumable>(_messageEncryptor.Decrypt(exchangeName, body));

                }
                catch (Exception ex)
                {
                    _log.Error("Failed to decrypt or deserialize message", ex);

                    throw;
                }

                using (var handler = _consumerFactory())
                {
                    response = handler.Value.Consume(message);

                    _log.Debug(string.Format("Successfully processed {0}", this.GetRoutingKey(typeof(TConsumable))));
                }

                CommonModel.BasicAck(deliveryTag, exchangeName, routingKey, false);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Failed to process {0} because {1}", this.GetRoutingKey(typeof(TConsumable)), ex.Message), ex);

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