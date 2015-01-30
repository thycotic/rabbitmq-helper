using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.MemoryMq;
using Thycotic.MessageQueueClient.RabbitMq;
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
        
        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        public IMemoryMqModel Model { get; set; }

        private readonly IMemoryMqConnection _connection;
        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqConsumerWrapperBase<TRequest, THandler>));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqConsumerWrapperBase{TRequest,THandler}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected MemoryMqConsumerWrapperBase(IMemoryMqConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Starts the consuming process.
        /// </summary>
        public void StartConsuming()
        {
            var queueName = this.GetQueueName(typeof(THandler), typeof(TRequest));

            var model = _connection.OpenChannel();

            _log.Debug(string.Format("Channel opened for {0}", queueName));

            model.QueueDeclare(queueName);
            model.QueueBind(queueName);

            const bool noAck = false; //since this consumer will send an acknowledgement
            var consumer = this;

            model.BasicConsume(queueName, noAck, consumer); //we will ack, hence no-ack=false

            Model = model;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        
        }
    }
}
