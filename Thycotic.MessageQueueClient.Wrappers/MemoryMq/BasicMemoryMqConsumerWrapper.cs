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


        ///// <summary>
        ///// Consumes the specified body.
        ///// </summary>
        ///// <param name="body">The body.</param>
        //public override void Consume(byte[] body)
        //{
        //    using (LogContext.Create("Processing message..."))
        //    {
        //        var message = _serializer.ToRequest<TRequest>(body);

        //        using (var handler = _handlerFactory())
        //        {
        //            handler.Value.Consume(message);
        //        }
        //    }
        //}
    }
}
