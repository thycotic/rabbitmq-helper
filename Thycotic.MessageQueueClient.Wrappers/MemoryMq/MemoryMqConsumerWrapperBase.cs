using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.MemoryMq;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers.MemoryMq
{
    /// <summary>
    /// Base consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public abstract class MemoryMqConsumerWrapperBase<TRequest, THandler> : IConsumerWrapperBase
        where TRequest : IConsumable
    {
        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqConsumerWrapperBase<TRequest, THandler>));

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqConsumerWrapperBase{TRequest,THandler}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected MemoryMqConsumerWrapperBase(IMemoryMqConnection connection)
        {
        }

        /// <summary>
        /// Starts the consuming process.
        /// </summary>
        public void StartConsuming()
        {
        }


        /// <summary>
        /// Handles the basic deliver.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        public abstract void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey,
            IBasicProperties properties, byte[] body);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        
        }
    }
}
