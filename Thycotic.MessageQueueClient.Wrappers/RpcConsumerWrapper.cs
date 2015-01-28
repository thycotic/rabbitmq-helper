using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.RabbitMq;

namespace Thycotic.MessageQueueClient.Wrappers
{
    public class AutofacRpcConsumer<TMsg, TResponse, THandler> : ConsumerWrapperBase<TMsg, THandler>
        where THandler : IConsumer<TMsg, TResponse>
    {
        //private readonly Func<Owned<IConsume<TMsg, TResponse>>> _handlerFactory;
        private readonly IMessageSerializer _serializer;
        private readonly IRabbitMqConnection _rmq;
        //private readonly IActivityMonitor _monitor;
        private readonly ILogWriter _log = Log.Get(typeof (AutofacRpcConsumer<TMsg, TResponse, THandler>));

        public AutofacRpcConsumer(IMessageSerializer serializer, IRabbitMqConnection rmq)
            : base(rmq)
        {

            _serializer = serializer;
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

                var message = _serializer.BytesToMessage<TMsg>(body);
                var responseType = "success";
                object response;

                //using (var handler = _handlerFactory())
                //{
                //    try
                //    {
                //        response = handler.Value.Consume(message);
                //    }
                //    catch (Exception e)
                //    {
                //        _log.Error("Handler error", e);
                //        response = new RpcError {Message = e.Message};
                //        responseType = "error";
                //    }
                //}

                //if (properties.IsReplyToPresent())
                //{
                //    Respond(properties.ReplyTo, response, properties.CorrelationId, responseType);
                //}
            }
            catch (Exception e)
            {
                _log.Error("Failed to handle message " + typeof (TMsg).Name, e);
            }
            finally
            {
                Model.BasicAck(deliveryTag, false);
            }
        }

        private void Respond(string replyTo, object response, string correlationId, string type)
        {
            var respBytes = _serializer.MessageToBytes(response);
            using (var channel = _rmq.OpenChannel(retryAttempts: 9, retryDelayMs: 125, retryDelayGrowthFactor: 2))
            {
                channel.ConfirmSelect();
                var p = channel.CreateBasicProperties();
                p.CorrelationId = correlationId;
                p.Type = type;
                channel.BasicPublish(exchange: "", routingKey: replyTo, mandatory: false, immediate: false,
                    basicProperties: p,
                    body: respBytes);
                channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);
            }
        }

    }

}