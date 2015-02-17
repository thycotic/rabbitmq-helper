using System;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueueClient.QueueClient
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBus" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="objectSerializer">The message serializer.</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        public RequestBus(ICommonConnection connection, IObjectSerializer objectSerializer, IMessageEncryptor messageEncryptor)
        {
            _connection = connection;
            _objectSerializer = objectSerializer;
            _messageEncryptor = messageEncryptor;
        }

        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="request">The request.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        public void BasicPublish(string exchangeName, IConsumable request, bool persistent = true)
        {
            _log.Debug(string.Format("Publishing basic (fire and forget) {0}", request.GetType()));

            var routingKey = request.GetRoutingKey();

            try
            {
                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    channel.ConfirmSelect();
                    channel.ExchangeDeclare(exchangeName, DefaultConfigValues.ExchangeType);

                    var properties = channel.CreateBasicProperties();
                    properties.SetPersistent(persistent);

                    channel.BasicPublish(exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory,
                        DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties,
                        _messageEncryptor.Encrypt(exchangeName, _objectSerializer.ToBytes(request)));

                    channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);
                }
            }
            catch (Exception e)
            {
                _log.Error("Failed to publish message", e);
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
        public TResponse BlockingPublish<TResponse>(string exchangeName, IConsumable request, int timeoutSeconds)
        {
            _log.Debug(string.Format("Publishing blocking {0}", request.GetType()));

            var routingKey = request.GetRoutingKey();

            try
            {
                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    using (var subscription = channel.CreateSubscription(channel.QueueDeclare().QueueName))
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

                        if (response.BasicProperties.Type != "error")
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
                _log.Error("RPC call failed", ex);
                throw;
            }
        }
    }
}