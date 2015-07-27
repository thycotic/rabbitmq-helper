using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Base consumer wrapper
    /// </summary>
    /// <typeparam name="TConsumable">The type of the request.</typeparam>
    /// <typeparam name="TConsumer">The type of the handler.</typeparam>
    public abstract class ConsumerWrapperBase<TConsumable, TConsumer> : IConsumerWrapperBase
        where TConsumable : IConsumable
    {
        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        public ICommonModel CommonModel { get; private set; }

        private readonly ICommonConnection _connection;
        private readonly IExchangeNameProvider _exchangeNameProvider;

        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _queueName;

        private bool _terminated;

        private readonly ILogWriter _log = Log.Get(typeof(TConsumer));
        private bool _disposed;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerWrapperBase{TConsumable, TConsumer}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="exchangeNameProvider">The exchange name provider.</param>
        protected ConsumerWrapperBase(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider)
        {
            Contract.Requires<ArgumentNullException>(connection != null);
            Contract.Requires<ArgumentNullException>(exchangeNameProvider != null);

            _connection = connection;
            _exchangeNameProvider = exchangeNameProvider;
            _connection.ConnectionCreated += (sender, args) => CreateModel();

            _exchangeName = _exchangeNameProvider.GetCurrentExchange();

            _routingKey = this.GetRoutingKey(typeof(TConsumable));

            _queueName = this.GetQueueName(_exchangeName, typeof(TConsumer), typeof(TConsumable));
        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns></returns>
        protected void CreateModel()
        {
            try
            {
                if (CommonModel != null)
                {
                    CommonModel.Dispose();
                    CommonModel = null;
                }

                using (LogContext.Create(_queueName))
                using (LogContext.Create("Creating model"))
                {
                    const int retryAttempts = -1; //forever
                    const int retryDelayGrowthFactor = 1;

                    var model = _connection.OpenChannel(retryAttempts, Convert.ToInt32(DefaultConfigValues.ReOpenDelay.TotalMilliseconds),
                        retryDelayGrowthFactor);

                    //TODO: Re-enable when Memory Mq honors this -dkk
                    //const int prefetchSize = 0;
                    //const int prefetchCount = 1;
                    //const bool global = false;

                    //model.BasicQos(prefetchSize, prefetchCount, global);

                    model.ModelShutdown += RecoverConnection;

                    model.ExchangeDeclare(_exchangeName, DefaultConfigValues.ExchangeType);

                    model.QueueDeclare(_queueName, true, false, false, null);
                    model.QueueBind(_queueName, _exchangeName, _routingKey);

                    const bool noAck = false; //since this consumer will send an acknowledgement
                    var consumer = this;

                    model.BasicConsume(_queueName, noAck, consumer); //we will ack, hence no-ack=false

                    _log.Info("Model ready");

                    CommonModel = model;
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Could not create model because {0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// Starts the consuming process.
        /// </summary>
        public void StartConsuming()
        {
            using (LogContext.Create(_queueName))
            using (LogContext.Create("Start consuming"))
            {
                try
                {
                    _connection.ResetConnection();
                }
                catch (Exception ex)
                {
                    //if there is an issue opening the channel, clean up and rethrow
                    _log.Error(string.Format("Failed to establish connection because {0}", ex.Message));

                    _log.Info(string.Format("Sleeping for {0}s before reconnecting", DefaultConfigValues.ReOpenDelay.TotalSeconds));

                    Task.Delay(DefaultConfigValues.ReOpenDelay).ContinueWith(task => StartConsuming());
                }
            }
        }

        private void RecoverConnection(object model, ModelShutdownEventArgs reason)
        {
            using (LogContext.Create(_queueName))
            using (LogContext.Create("Recovery"))
            {
                if (_terminated)
                {
                    return;
                }

                _log.Warn(string.Format("Channel closed because {0}", reason.ReplyText));

                Task.Delay(DefaultConfigValues.ReOpenDelay).ContinueWith(task =>
                {
                    _log.Info("Reopening channel...");
                    StartConsuming();
                });
            }
        }

        /// <summary>
        /// Starts the handle task.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        protected abstract Task StartHandleTask(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey,
            ICommonModelProperties properties, byte[] body);

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
        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey,
            ICommonModelProperties properties, byte[] body)
        {
            StartHandleTask(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _terminated = true;

            if (CommonModel == null || !CommonModel.IsOpen) return;

            _log.Debug("Closing channel...");
            CommonModel.Dispose();
            _log.Debug("Channel closed");

            _log.Debug("Closing connection...");
            _connection.Dispose();
            _log.Debug("Connection closed");

            _disposed = true;
        }
    }
}
