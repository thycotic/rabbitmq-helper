using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers
{
    public class RpcConsumerWrapper<TRequest, TResponse, THandler> : ConsumerWrapperBase<TRequest, THandler>
        where TRequest: IConsumable
        where THandler : IRpcConsumer<TRequest, TResponse>
    {
        //private readonly Func<Owned<IConsume<TMsg, TResponse>>> _handlerFactory;
        private readonly IMessageSerializer _serializer;
        private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly IRabbitMqConnection _rmq;
        //private readonly IActivityMonitor _monitor;
        private readonly ILogWriter _log = Log.Get(typeof (RpcConsumerWrapper<TRequest, TResponse, THandler>));

        public RpcConsumerWrapper(IRabbitMqConnection rmq, IMessageSerializer serializer, Func<Owned<THandler>> handlerFactory)
            : base(rmq)
        {

            _serializer = serializer;
            _handlerFactory = handlerFactory;
            _rmq = rmq;

        }

         public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey, IBasicProperties properties, byte[] body)
        {
            Task.Run(() => ExecuteMessage(deliveryTag, properties, body));
        }

        public void ExecuteMessage(ulong deliveryTag, IBasicProperties properties, byte[] body)
        {
            try
            {

                var message = _serializer.BytesToMessage<TRequest>(body);
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
                        response = new RpcError { Message = e.Message };
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
            var body = _serializer.MessageToBytes(response);
            var routingKey = replyTo;

            using (var channel = _rmq.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
            {
                channel.ConfirmSelect();

                var properties = channel.CreateBasicProperties();

                properties.CorrelationId = correlationId;
                properties.Type = type;

                //TODO: Shoudl this be empty or the default exchange
                var exchange = string.Empty;

                channel.BasicPublish(exchange, routingKey, DefaultConfigValues.Model.Publish.NotMandatory, DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties, body);
                
                channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);
            }
        }
    }
}