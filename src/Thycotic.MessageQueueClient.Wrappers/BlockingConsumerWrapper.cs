using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers
{
    /// <summary>
    /// RPC consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public class BlockingConsumerWrapper<TRequest, TResponse, THandler> : ConsumerWrapperBase<TRequest, THandler>
        where TRequest : class, IConsumable
        where TResponse : class
        where THandler : IBlockingConsumer<TRequest, TResponse>
    {
        private readonly IMessageSerializer _serializer;
        private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly ICommonConnection _connection;
        private readonly ILogWriter _log = Log.Get(typeof(BlockingConsumerWrapper<TRequest, TResponse, THandler>));

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockingConsumerWrapper{TRequest,TResponse,THandler}" /> class.
        /// </summary>
        /// <param name="connection">The RMQ.</param>
        /// <param name="exchangeNameProvider">The exchange provider.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        public BlockingConsumerWrapper(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IMessageSerializer serializer, Func<Owned<THandler>> handlerFactory)
            : base(connection, exchangeNameProvider)
        {

            _serializer = serializer;
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
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        private void ExecuteMessage(ulong deliveryTag, string exchange, string routingKey, ICommonModelProperties properties, byte[] body)
        {
            try
            {

                var message = _serializer.ToRequest<TRequest>(body);
                var responseType = "success";
                object response;

                using (var handler = _handlerFactory())
                {
                    try
                    {
                        response = handler.Value.Consume(message);

                        _log.Debug(string.Format("Successfully processed {0}", this.GetRoutingKey(typeof(TRequest))));
                    }
                    catch (Exception e)
                    {
                        _log.Error("Handler error", e);
                        response = new BlockingConsumerError { Message = e.Message };
                        responseType = "error";
                    }
                }

                if (properties.IsReplyToPresent())
                {
                    Respond(properties.ReplyTo, response, properties.CorrelationId, responseType);
                }
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Failed to process {0}", this.GetRoutingKey(typeof(TRequest))), e);
            }
            finally
            {
                CommonModel.BasicAck(deliveryTag, exchange, routingKey, false);
            }
        }

        private void Respond(string replyTo, object response, string correlationId, string type)
        {
            var body = _serializer.ToBytes(response);
            var routingKey = replyTo;

            using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
            {
                channel.ConfirmSelect();

                var properties = channel.CreateBasicProperties();

                properties.CorrelationId = correlationId;
                properties.Type = type;

                //TODO: Should this be empty or the default exchange
                var exchange = string.Empty;

                channel.BasicPublish(exchange, routingKey, DefaultConfigValues.Model.Publish.NotMandatory, DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties, body);

                channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);
            }
        }
    }
}