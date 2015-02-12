using System;
using Thycotic.Logging;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Rabbit Mq message bus
    /// </summary>
    public class RequestBus : IRequestBus
    {
        private readonly ICommonConnection _connection;
        private readonly IExchangeNameProvider _exchangeNameProvider;
        private readonly IMessageSerializer _messageSerializer;

        private readonly ILogWriter _log = Log.Get(typeof(RequestBus));

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBus" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="exchangeNameProvider">The exchange provider.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        public RequestBus(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IMessageSerializer messageSerializer)
        {
            _connection = connection;
            _exchangeNameProvider = exchangeNameProvider;
            _messageSerializer = messageSerializer;
        }
        
        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        public void BasicPublish(IConsumable request, bool persistent = true)
        {
            _log.Debug(string.Format("Publishing basic (fire and forget) {0}", request));

            var body = _messageSerializer.ToBytes(request);
            var routingKey = request.GetRoutingKey();

            try
            {
                var exchangeName = _exchangeNameProvider.GetCurrentChange();

                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    channel.ConfirmSelect();
                    channel.ExchangeDeclare(exchangeName, DefaultConfigValues.ExchangeType);

                    var properties = channel.CreateBasicProperties();
                    properties.SetPersistent(persistent);

                    channel.BasicPublish(exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory, DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties, body);

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
        /// <param name="request">The request.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">
        /// Blocking call timed out
        /// or
        /// Blocking call was disconnected
        /// or
        /// CorrelationId mismatch
        /// or
        /// </exception>
        public TResponse BlockingPublish<TResponse>(IConsumable request, int timeoutSeconds)
        {
            _log.Debug(string.Format("Publishing blocking {0}", request));

            var body = _messageSerializer.ToBytes(request);
            var routingKey = request.GetRoutingKey();

            try
            {
                var exchangeName = _exchangeNameProvider.GetCurrentChange();

                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    using (var subscription = channel.CreateSubscription(channel.QueueDeclare().QueueName))
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.CorrelationId = Guid.NewGuid().ToString();
                        properties.ReplyTo = subscription.QueueName;
                        channel.ConfirmSelect();
                        channel.ExchangeDeclare(exchangeName, DefaultConfigValues.ExchangeType);

                        channel.BasicPublish(exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory, DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties, body);

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
                            return _messageSerializer.ToRequest<TResponse>(response.Body);
                        }

                        var error = _messageSerializer.ToRequest<BlockingConsumerError>(response.Body);
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