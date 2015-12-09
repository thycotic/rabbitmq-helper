using System;
using System.Diagnostics.Contracts;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Request bus
    /// </summary>
    public class RequestBus : IRequestBus
    {
        private readonly ICommonConnection _connection;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMessageEncryptor _messageEncryptor;

        private readonly ILogWriter _log = Log.Get(typeof(RequestBus));
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBus" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="objectSerializer">The message serializer.</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        public RequestBus(ICommonConnection connection, IObjectSerializer objectSerializer, IMessageEncryptor messageEncryptor)
        {
            Contract.Requires<ArgumentNullException>(connection != null);
            Contract.Requires<ArgumentNullException>(objectSerializer != null); 
            Contract.Requires<ArgumentNullException>(messageEncryptor != null);

            _connection = connection;
            _objectSerializer = objectSerializer;
            _messageEncryptor = messageEncryptor;
            ServerVersion = connection.ServerVersion;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServerVersion { get; private set; }

        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="request">The request.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        public void BasicPublish(string exchangeName, IBasicConsumable request, bool persistent = true)
        {
            _log.Debug(string.Format("Publishing basic (fire and forget) {0}", request.GetType()));

            var routingKey = request.GetRoutingKey();

            try
            {
                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, Convert.ToInt32(DefaultConfigValues.Model.RetryDelay.TotalMilliseconds), DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    channel.ConfirmSelect();
                    channel.ExchangeDeclare(exchangeName, DefaultConfigValues.ExchangeType);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = persistent;

                    channel.BasicPublish(exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory,
                        DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties,
                        _messageEncryptor.Encrypt(exchangeName, _objectSerializer.ToBytes(request)));

                    channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Basic publish failed because {0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// Publishes the specified request as an RPC.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="request">The request.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Blocking call timed out
        /// or
        /// Blocking call was disconnected
        /// or
        /// CorrelationId mismatch
        /// or</exception>
        public TResponse BlockingPublish<TResponse>(string exchangeName, IBlockingConsumable request, int timeoutSeconds)
        {
            _log.Debug(string.Format("Publishing blocking {0}", request.GetType()));

            var routingKey = request.GetRoutingKey();

            try
            {
                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, Convert.ToInt32(DefaultConfigValues.Model.RetryDelay.TotalMilliseconds), DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    var queueName = channel.AutoDeleteQueueDeclare().QueueName;

                    using (var subscription = channel.CreateSubscription(queueName))
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.CorrelationId = Guid.NewGuid().ToString();
                        properties.ReplyTo = subscription.QueueName;
                        channel.ConfirmSelect();
                        channel.ExchangeDeclare(exchangeName, DefaultConfigValues.ExchangeType);

                        channel.BasicPublish(exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory,
                            DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties,
                            _messageEncryptor.Encrypt(exchangeName, _objectSerializer.ToBytes(request)));

                        channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);

                        CommonDeliveryEventArgs response;
                        if (!subscription.Next(timeoutSeconds * 1000, out response))
                        {
                            throw new ApplicationException("Blocking call timed out");
                        }

                        if (response == null)
                        {
                            throw new ApplicationException("Blocking call was disconnected");
                        }

                        if (response.BasicProperties.CorrelationId != properties.CorrelationId)
                        {
                            throw new ApplicationException("CorrelationId mismatch");
                        }

                        if (response.BasicProperties.ResponseType != BlockingConsumerResponseTypes.Error)
                        {
                            return _objectSerializer.ToObject<TResponse>(_messageEncryptor.Decrypt(exchangeName, response.Body));
                        }

                        var error = _objectSerializer.ToObject<BlockingConsumerError>(_messageEncryptor.Decrypt(exchangeName, response.Body));
                        throw new ApplicationException(error.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Blocking publish failed because {0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// Disposes the bus
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            //do not dispose the connection
            _disposed = true;
        }
    }
}