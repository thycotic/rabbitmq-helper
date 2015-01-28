using System;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Thycotic.Logging;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public class RabbitMqMessageBus : IMessageBus
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IMessageSerializer _messageSerializer;
        private readonly string _exchangeName;

        private readonly ILogWriter _log = Log.Get(typeof(RabbitMqMessageBus));

        public RabbitMqMessageBus(IRabbitMqConnection connection, IMessageSerializer messageSerializer,
                                string exchangeName = DefaultConfigValues.Exchange)
        {
            _connection = connection;
            _messageSerializer = messageSerializer;
            _exchangeName = exchangeName;
        }

        public TResponse Rpc<TResponse>(IConsumable request, int timeoutSeconds)
        {
            _log.Debug(string.Format("Publishing RPC {0}", request));

            var body = _messageSerializer.MessageToBytes(request);
            var routingKey = request.GetRoutingKey();

            try
            {
                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    using (var subscription = new Subscription(channel, channel.QueueDeclare().QueueName))
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.CorrelationId = Guid.NewGuid().ToString();
                        properties.ReplyTo = subscription.QueueName;
                        channel.ConfirmSelect();
                        channel.ExchangeDeclare(_exchangeName, DefaultConfigValues.ExchangeType);

                        channel.BasicPublish(_exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory, DefaultConfigValues.Model.Publish.DeliverImmediatelyAndRequireAListener, properties, body);

                        channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);

                        BasicDeliverEventArgs response;
                        if (!subscription.Next(timeoutSeconds * 1000, out response))
                        {
                            throw new ApplicationException("RPC call timed out");
                        }

                        if (response == null)
                        {
                            throw new ApplicationException("RPC call was disconnected");
                        }
                        if (response.BasicProperties.CorrelationId != properties.CorrelationId)
                        {
                            throw new ApplicationException("CorrelationId mismatch");
                        }
                        if (response.BasicProperties.Type == "error")
                        {
                            //var error = _messageSerializer.BytesToMessage<RpcError>(response.Body);
                            throw new ApplicationException("RPC call failed: ");// + error.Message);
                        }
                        return _messageSerializer.BytesToMessage<TResponse>(response.Body);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("RPC call failed", ex);
                throw;
            }
        }

        public void Publish(IConsumable request, bool persistent = true)
        {
            _log.Debug(string.Format("Publishing basic {0}", request));

            var body = _messageSerializer.MessageToBytes(request);
            var routingKey = request.GetRoutingKey();

            try
            {
                using (var channel = _connection.OpenChannel(DefaultConfigValues.Model.RetryAttempts, DefaultConfigValues.Model.RetryDelayMs, DefaultConfigValues.Model.RetryDelayGrowthFactor))
                {
                    channel.ConfirmSelect();
                    channel.ExchangeDeclare(_exchangeName, DefaultConfigValues.ExchangeType);

                    var properties = channel.CreateBasicProperties();
                    properties.SetPersistent(persistent);

                    channel.BasicPublish(_exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory, DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, properties, body);

                    channel.WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);
                }
            }
            catch (Exception e)
            {
                _log.Error("Failed to publish message", e);
                throw;
            }
        }


    }
}