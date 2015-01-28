using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers
{
    public class SimpleConsumerWrapper<TRequest, THandler> : ConsumerWrapperBase<TRequest, THandler>
        where TRequest : IConsumable
        where THandler : IConsumer<TRequest>
    {
        private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly IMessageSerializer _serializer;
        private readonly ILogWriter _log = Log.Get(typeof (SimpleConsumerWrapper<TRequest, THandler>));

        public SimpleConsumerWrapper(IRabbitMqConnection rmq, IMessageSerializer serializer, Func<Owned<THandler>> handlerFactory)
            : base(rmq)
        {
            _handlerFactory = handlerFactory;
            _serializer = serializer;
        }


        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey, IBasicProperties properties, byte[] body)
        {
            Task.Run(() => ExecuteMessage(deliveryTag, body));
        }

        public void ExecuteMessage(ulong deliveryTag, byte[] body)
        {
            const bool multiple = false;

            using (LogContext.Create("Processing message..."))
            {

                try
                {

                    var message = _serializer.BytesToMessage<TRequest>(body);

                    using (var handler = _handlerFactory())
                    {
                        handler.Value.Consume(message);
                    }

                    //_log.Debug(string.Format("Successfully processed {0}", this.GetRoutingKey(typeof(TRequest))));

                    Model.BasicAck(deliveryTag, multiple);

                }
                catch (Exception e)
                {
                    _log.Error(string.Format("Failed to process {0}", this.GetRoutingKey(typeof (TRequest))), e);

                    Model.BasicNack(deliveryTag, multiple, requeue: true);
                }
            }

        }
    }
}
