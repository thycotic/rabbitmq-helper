﻿using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers.RabbitMq
{
    /// <summary>
    /// RPC consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public class BlockingRabbitMqConsumerWrapper<TRequest, TResponse, THandler> : RabbitMqConsumerWrapperBase<TRequest, THandler>
        where TRequest: IConsumable
        where THandler : IBlockingConsumer<TRequest, TResponse>
    {
        private readonly IMessageSerializer _serializer;
        private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly IRabbitMqConnection _connection;
        private readonly ILogWriter _log = Log.Get(typeof (BlockingRabbitMqConsumerWrapper<TRequest, TResponse, THandler>));

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockingRabbitMqConsumerWrapper{TRequest,TResponse,THandler}"/> class.
        /// </summary>
        /// <param name="connection">The RMQ.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        public BlockingRabbitMqConsumerWrapper(IRabbitMqConnection connection, IMessageSerializer serializer, Func<Owned<THandler>> handlerFactory)
            : base(connection)
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
            string routingKey, IBasicProperties properties, byte[] body)
        {
            Task.Run(() => ExecuteMessage(deliveryTag, properties, body));
        }

         /// <summary>
         /// Executes the message.
         /// </summary>
         /// <param name="deliveryTag">The delivery tag.</param>
         /// <param name="properties">The properties.</param>
         /// <param name="body">The body.</param>
        public void ExecuteMessage(ulong deliveryTag, IBasicProperties properties, byte[] body)
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
                Model.BasicAck(deliveryTag, false);
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