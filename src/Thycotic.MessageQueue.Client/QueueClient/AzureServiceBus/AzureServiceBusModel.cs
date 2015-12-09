using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.Wrappers;

namespace Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus
{

    //exchange - topic
    //routingKey - routing key
    //queue - NOT USED

    /// <summary>
    /// Azure service bus model
    /// </summary>
    public class AzureServiceBusModel : ICommonModel
    {
        private readonly IAzureServiceBusConnection _connection;
        private MessageReceiver _requestClient;
        private readonly object _syncRoot = new object();
        private long _currentDeliveryCount;

        private readonly ILogWriter _log = Log.Get(typeof(AzureServiceBusModel));

        private Task _consumeTask;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusModel" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public AzureServiceBusModel(IAzureServiceBusConnection connection)
        {
            _connection = connection;
            IsOpen = true;
        }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue {
            get { return null; }
        }


        /// <summary>
        /// Gets or sets the model shutdown.
        /// </summary>
        /// <value>
        /// The model shutdown.
        /// </value>
        public EventHandler<ModelShutdownEventArgs> ModelShutdown { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen { get; set; }

        #region Mapping
        private static AzureServiceBusModelProperties Map(BrokeredMessage message)
        {
            return new AzureServiceBusModelProperties(message);
        }
        #endregion

        /// <summary>
        /// Creates the basic properties.
        /// </summary>
        /// <returns></returns>
        public ICommonModelProperties CreateBasicProperties()
        {
            return new AzureServiceBusModelProperties();
        }

        /// <summary>
        /// Confirms the select.
        /// </summary>
        public void ConfirmSelect()
        {
            //nothing here
        }
        
        /// <summary>
        /// Declares the specified topic.
        /// </summary>
        /// <param name="exchangeName">The topic path.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        public void ExchangeDeclare(string exchangeName, string exchangeType)
        {
            var manager = _connection.CreateManager();
            manager.CreateTopic(exchangeName);
        }
        
        /// <summary>
        /// Publishes to the topic.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="immediate">if set to <c>true</c> [do not deliver immediately or require a listener].</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate,
            ICommonModelProperties properties, byte[] body)
        {

            var azureProperties = (AzureServiceBusModelProperties)properties;
            azureProperties.Exchange = exchangeName;
            azureProperties.RoutingKey = routingKey;

            azureProperties.SetBytes(body);

            var message = properties.GetRawValue<BrokeredMessage>();

            //this is a directed message without an exchange
            if (string.IsNullOrEmpty(exchangeName))
            {
                var messageSender = _connection.CreateSender(routingKey);
                messageSender.Send(message);
            }
            else
            {
                var topicClient = _connection.CreateTopicClient(exchangeName);
                topicClient.Send(message);
            }
        }

        /// <summary>
        /// Wait until all messages published since the last call have been either completed or abandoned by the broker.
        /// </summary>
        /// <param name="confirmationTimeout">The confirmation timeout.</param>
        public void WaitForConfirmsOrDie(TimeSpan confirmationTimeout)
        {
            //TODO: Wait until the messages on the bus are published
        }

        /// <summary>
        /// Basic quality of service.
        /// </summary>
        /// <param name="prefetchSize">Size of the prefetch.</param>
        /// <param name="prefetchCount">The prefetch count.</param>
        /// <param name="global">if set to <c>true</c> global.</param>
        public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            //nothing here
        }

        /// <summary>
        /// Basics the ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        public void BasicAck(DeliveryTagWrapper deliveryTag, string exchange, string routingKey, bool multiple)
        {
            _requestClient.Complete(deliveryTag);
        }

        /// <summary>
        /// Basics the nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        public void BasicNack(DeliveryTagWrapper deliveryTag, string exchange, string routingKey, bool multiple, bool requeue)
        {
            if (requeue)
            {
                _requestClient.Abandon(deliveryTag);
            }
            else
            {
                _requestClient.DeadLetter(deliveryTag);
            }
        }

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <returns></returns>
        public ICommonQueue QueueDeclare()
        {
            var queueName = Guid.NewGuid().ToString();
            return QueueDeclare(queueName, true, false, false, null);
        }

        /// <summary>
        /// Queues the declare.
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="durable"></param>
        /// <param name="exclusive"></param>
        /// <param name="autoDelete"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICommonQueue QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            var manager = _connection.CreateManager();
            manager.CreateQueue(queueName);

            return new AzureServiceBusQueue(queueName);
        }

        /// <summary>
        /// Queues the bind.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            var manager = _connection.CreateManager();
            manager.CreateRoutingKeyQueueSubscription(queueName, exchangeName, routingKey);
        }

        /// <summary>
        /// Creates the subscription to the model.
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public ISubscription CreateSubscription(string queueName)
        {
            return new AzureServiceBusSubscription(queueName, _connection);
        }

        /// <summary>
        /// Basics the consume.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="consumer">The consumer.</param>
        /// <exception cref="System.ApplicationException">Already consuming</exception>
        public void BasicConsume(CancellationToken token, string queueName, bool noAck, IConsumerWrapperBase consumer)
        {
            //no need to look up the queue

            lock (_syncRoot)
            {
                if (_consumeTask != null)
                {
                    throw new ApplicationException("Already consuming");
                }
            }

            _requestClient = _connection.CreateReceiver(queueName);

            _consumeTask = Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var message = _requestClient.Receive(TimeSpan.FromSeconds(5));

                        if (message == null)
                        {
                            continue;
                        }

                        _log.Debug(string.Format("Processing message {0} with lock token {1}", message.MessageId, message.LockToken));

                        if (message.DeliveryCount > 5)
                        {
                            _log.Error(string.Format("Throwing away message {0} for {1} because too many deliveries", message.MessageId, _requestClient.Path));
                            _requestClient.DeadLetter(message.LockToken, "Too many redeliveries", string.Empty);
                            continue;
                        }

                        var properties = Map(message);

                        var messageForTask = message;
                        Task.Factory
                            .StartNew(() =>
                            {
                                try
                                {
                                    Interlocked.Increment(ref _currentDeliveryCount);

                                    consumer.HandleBasicDeliver(string.Empty, new DeliveryTagWrapper(messageForTask.LockToken),
                                        messageForTask.DeliveryCount > 0,
                                        properties.Exchange,
                                        properties.RoutingKey,
                                        properties, messageForTask.GetBytes());
                                }
                                finally
                                {
                                    Interlocked.Decrement(ref _currentDeliveryCount);
                                }

                            }, token)
                            .ContinueWith(task =>
                            {
                                if (task.Exception != null)
                                {
                                    _log.Error("Failed to deliver message", task.Exception);
                                }
                            }, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Failed to deliver message. Waiting 15 seconds...", ex);
                        try
                        {
                            Task.Delay(TimeSpan.FromSeconds(15), token).Wait(token);
                        }
                        catch (OperationCanceledException)
                        {
                            //token was cancelled while waiting to retry
                        }
                    }
                }

                _log.Info("Consuming cancelled");

            }, CancellationToken.None);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            //nothing to close
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (_syncRoot)
            {
                if (_disposed)
                {
                    return;
                }

                var tries = 0;
                while (Interlocked.Read(ref _currentDeliveryCount) > 0)
                {
                    if (tries > 5)
                    {
                        _log.Error("Too many tries attempting to dispose model");
                    }

                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    tries++;
                }

                lock (_syncRoot)
                {
                    if (_consumeTask != null)
                    {
                        _consumeTask.Wait(TimeSpan.FromSeconds(10));
                    }
                }

                _requestClient = null;

                _disposed = true;
            }
        }
    }
}