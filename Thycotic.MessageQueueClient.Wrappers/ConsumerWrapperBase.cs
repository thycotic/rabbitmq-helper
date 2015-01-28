using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers
{
    /// <summary>
    /// Base consumer wrapper
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public abstract class ConsumerWrapperBase<TRequest, THandler> : IConsumerWrapperBase, IBasicConsumer
        where TRequest : IConsumable
    {
        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        public IModel Model { get; private set; }

        /// <summary>
        /// Signaled when the consumer gets cancelled.
        /// </summary>
#pragma warning disable 0067 //disable never used warning
        public event ConsumerCancelledEventHandler ConsumerCancelled;
#pragma warning restore 0067

        private readonly IRabbitMqConnection _connection;

        private bool _terminated;

        private readonly ILogWriter _log = Log.Get(typeof(ConsumerWrapperBase<TRequest, THandler>));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerWrapperBase{TRequest, THandler}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected ConsumerWrapperBase(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Starts the consuming process.
        /// </summary>
        public void StartConsuming()
        {
            var routingKey = this.GetRoutingKey(typeof (TRequest));

            var queueName =  this.GetQueueName(typeof(THandler), typeof(TRequest));
            _log.Debug(string.Format("Channel opened for {0}", queueName));

            const int retryAttempts = -1; //forever
            const int retryDelayGrowthFactor = 1;

            var model = _connection.OpenChannel(retryAttempts, DefaultConfigValues.ReOpenDelay, retryDelayGrowthFactor);

            const int prefetchSize = 0;
            const int prefetchCount = 1;
            const bool global = false;
            
            model.BasicQos(prefetchSize, prefetchCount, global);

            model.ModelShutdown += RecoverConnection;

            model.ExchangeDeclare(DefaultConfigValues.Exchange, DefaultConfigValues.ExchangeType);
            model.QueueDeclare(queueName, true, false, false, null);
            model.QueueBind(queueName, DefaultConfigValues.Exchange, routingKey);

            const bool noAck = false; //since this consumer will send an acknowledgement
            var consumer = this;

            model.BasicConsume(queueName, noAck, consumer); //we will ack, hence no-ack=false

            Model = model;

        }

        private void RecoverConnection(IModel model, ShutdownEventArgs reason)
        {
            if (_terminated) return;

            _log.Warn(string.Format("Channel closed because {0}", reason));

            _log.Debug(string.Format("Will reopen in {0}(s)", DefaultConfigValues.ReOpenDelay/1000));

            Task.Delay(DefaultConfigValues.ReOpenDelay).ContinueWith(_ =>
            {
                _log.Debug("Reopenning channel...");
                StartConsuming();
            });

        }

        #region Not implemented/needed
        /// <summary>
        /// Called upon successful registration of the
        /// consumer with the broker.
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicConsumeOk(string consumerTag)
        {
            //not needed but forced by the interface
        }

        /// <summary>
        /// Called upon successful deregistration of the
        /// consumer from the broker.
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicCancelOk(string consumerTag)
        {
            //not needed but forced by the interface
        }

        /// <summary>
        /// Called when the consumer is cancelled for reasons other than by a
        /// basicCancel: e.g. the queue has been deleted (either by this channel or
        /// by any other channel). See handleCancelOk for notification of consumer
        /// cancellation due to basicCancel.
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicCancel(string consumerTag)
        {
            //not needed but forced by the interface
        }

        /// <summary>
        /// Called when the model shuts down.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="reason"></param>
        public void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            //not needed but forced by the interface
        }
        #endregion

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
        public abstract void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey,
            IBasicProperties properties, byte[] body);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _terminated = true;

            if (Model == null || !Model.IsOpen) return;

            _log.Debug("Closing channel...");
            Model.Close();
            _log.Debug("Channel closed");
        }
    }
}
