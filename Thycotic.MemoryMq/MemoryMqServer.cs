using System;
using System.ServiceModel;
using Thycotic.Logging;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class MemoryMqServer : IMemoryMqServer, IDisposable
    {
        private readonly Exchange _messages = new Exchange();
        private readonly Bindings _bindings = new Bindings();
        private readonly Clients _clients = new Clients();
        private readonly MessageDispatcher _messageDispatcher;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServer));

        public MemoryMqServer()
        {
            _messageDispatcher = new MessageDispatcher(_messages, _bindings, _clients);
            _messageDispatcher.Start();
        }

        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, byte[] body)
        {
            _messages.Publish(new RoutingSlip(exchangeName, routingKey), body);
        }

        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            _bindings.AddBinding(new RoutingSlip(exchangeName, routingKey), queueName);
        }

        public void BasicConsume(string queueName)
        {
            _log.Debug("Attaching consumer");
            
            _clients.AddConsumer(queueName);
        }

        public void BasicNack(ulong deliveryTag, bool multiple)
        {
            //TODO: Implement
        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            //TODO: Implement
        }

        public void Dispose()
        {
            _messageDispatcher.Stop();
        }
    }
}