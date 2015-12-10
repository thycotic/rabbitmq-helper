using System;
using System.Diagnostics.Contracts;
using System.Threading;
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
    /// <seealso cref="Thycotic.MessageQueue.Client.Wrappers.IConsumerWrapperBase" />
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

        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _queueName;

        private bool _recovering;
        private bool _terminated;
        private bool _disposed;

        private readonly object _syncRoot = new object();

        private readonly ILogWriter _log = Log.Get(typeof(TConsumer));
        private CancellationTokenSource _cts;

        /// <summary>
        /// PriorityScheduler to use (sets thread priority)
        /// </summary>
        protected PriorityScheduler PriorityScheduler { get; set; }
        

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
            _exchangeName = exchangeNameProvider.GetCurrentExchange();
            _routingKey = this.GetRoutingKey(typeof(TConsumable));
            _queueName = this.GetQueueName(_exchangeName, typeof(TConsumer), typeof(TConsumable));
        }

        /// <summary>
        /// Sets the priority
        /// </summary>
        /// <param name="scheduler"></param>
        public void SetPriority(IPriorityScheduler scheduler)
        {
            PriorityScheduler = (PriorityScheduler)scheduler;
        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns></returns>
        private void CreateModel()
        {
            lock (_syncRoot)
            {
                if (CommonModel != null)
                {
                    CommonModel.Dispose();
                }

                if (_cts != null)
                {
                    _cts.Cancel();
                }

                _cts = new CancellationTokenSource();

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

                _log.Info(string.Format("Channel opened for {0}", _queueName));
                
                const bool noAck = false; //since this consumer will send an acknowledgement
                var consumer = this;

                model.BasicConsume(_cts.Token, _queueName, noAck, consumer); //we will ack, hence no-ack=false

                CommonModel = model;
            }
        }

        /// <summary>
        /// Starts the consuming process.
        /// </summary>
        public void StartConsuming()
        {
            lock (_log)
            {
                _log.Debug(string.Format("Consuming {0}", _queueName));
            }
            CreateModel();
             
        }

        private void RecoverConnectionWorker(Exception ex)
        {
            _log.Warn(string.Format("Encountered issue with channel for {0}. Will reconnect in {1}ms. {2} Use DEBUG logging for more details.", _queueName, DefaultConfigValues.ReOpenDelay, ex.Message));
            _log.Debug(string.Format("Encountered issue with channel for {0}. Will reconnect in {1}ms", _queueName, DefaultConfigValues.ReOpenDelay), ex);

            Task.Delay(DefaultConfigValues.ReOpenDelay).ContinueWith(task =>
            {
                lock (_syncRoot)
                {
                    if (_terminated)
                    {
                        return;
                    }

                    if (!_recovering)
                    {
                        return;
                    }

                    StartConsuming();

                    //if we get to this line, there was no exception and consuming has begun
                    _recovering = false;
                }
            }).ContinueWith(task =>
            {
                if (task.Exception == null) return;

                RecoverConnectionWorker(task.Exception);
            });
        }

        private void RecoverConnection(object model, ModelShutdownEventArgs reason)
        {
            lock (_syncRoot)
            {
                if (_terminated)
                {
                    return;
                }

                if (_recovering)
                {
                    return;
                }
            
                _recovering = true;

            }

            RecoverConnectionWorker(new ApplicationException(reason.ReplyText));
            
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
        protected abstract Task StartHandleTask(string consumerTag, DeliveryTagWrapper deliveryTag, bool redelivered, string exchange,
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
        public void HandleBasicDeliver(string consumerTag, DeliveryTagWrapper deliveryTag, bool redelivered, string exchange,
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

            if (_cts != null)
            {
                _cts.Cancel();
            }

            _terminated = true;

            if (CommonModel == null || !CommonModel.IsOpen) return;

            _log.Debug("Closing channel...");
            CommonModel.Dispose();
            _log.Debug("Channel closed");
            CommonModel = null;

            //do not dispose the connection

            _disposed = true;
        }
    }
}
