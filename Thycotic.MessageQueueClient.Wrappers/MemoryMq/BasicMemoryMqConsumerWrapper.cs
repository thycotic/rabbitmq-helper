using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.MemoryMq;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers.MemoryMq
{
    /// <summary>
    /// Simple consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public class BasicMemoryMqConsumerWrapper<TRequest, THandler> : MemoryMqConsumerWrapperBase<TRequest, THandler>
        where TRequest : IConsumable
        where THandler : IBasicConsumer<TRequest>
    {
        private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly IMessageSerializer _serializer;
        private readonly ILogWriter _log = Log.Get(typeof(BasicMemoryMqConsumerWrapper<TRequest, THandler>));

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicMemoryMqConsumerWrapper{TRequest,THandler}"/> class.
        /// </summary>
        /// <param name="rmq">The RMQ.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        public BasicMemoryMqConsumerWrapper(IMemoryMqConnection rmq, IMessageSerializer serializer, Func<Owned<THandler>> handlerFactory)
            : base(rmq)
        {
            _handlerFactory = handlerFactory;
            _serializer = serializer;
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
            Task.Run(() => ExecuteMessage(deliveryTag, body));
        }

        /// <summary>
        /// Executes the message.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="body">The body.</param>
        public void ExecuteMessage(ulong deliveryTag, byte[] body)
        {
            //const bool multiple = false;

            using (LogContext.Create("Processing message..."))
            {
                try
                {
                    var message = _serializer.ToRequest<TRequest>(body);

                    using (var handler = _handlerFactory())
                    {
                        handler.Value.Consume(message);
                    }

                    //_log.Debug(string.Format("Successfully processed {0}", this.GetRoutingKey(typeof(TRequest))));

                    //Model.BasicAck(deliveryTag, multiple);

                }
                catch (Exception)
                {
                    //_log.Error(string.Format("Failed to process {0}", this.GetRoutingKey(typeof(TRequest))), e);

                    //Model.BasicNack(deliveryTag, multiple, requeue: true);
                }
            }

        }
    }
}
