using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Thycotic.Logging;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class MemoryMqServer : IMemoryMqServer
    {
        private readonly Exchange _messages = new Exchange();
        private readonly Bindings _bindings = new Bindings();
        private readonly Clients _clients = new Clients();
        private readonly MessageDispatcher _messageDispatcher;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServer));

        public MemoryMqServer()
        {
            _messageDispatcher = new MessageDispatcher(_messages, _bindings, _clients);
        }

        private static string GetFullRoutingSlip(string exchange, string routingKey)
        {
            return string.Format("{0}:{1}", exchange, routingKey);
        }

        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, byte[] body)
        {
            var fullRoutingSlip = GetFullRoutingSlip(exchangeName, routingKey);
            _messages.Publish(fullRoutingSlip, body);
        }

        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            _bindings.AddBinding(GetFullRoutingSlip(exchangeName, routingKey), queueName);
        }

        public void BasicConsume(string queueName)
        {
            _log.Debug("Attaching consumer");
            
            _clients.AddConsumer(queueName);
        }

        public void BasicNack(ulong deliveryTag, bool multiple)
        {

        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {

        }

        //public void BasicPublish(string meal)
        //{
        //    _callbackChannel = OperationContext.Current
        //        .GetCallbackChannel<IMemoryMqServiceCallback>();

        //    Console.WriteLine("Microwave Service");
        //    Console.WriteLine("Let's prepare us some {0}", meal);

        //    _counter = 999;
        //    _timer = new Timer(BasicAck, null, 500, 500);
        //}

        //public void BlockingPublish(string meal)
        //{
        //    _callbackChannel = OperationContext.Current
        //        .GetCallbackChannel<IMemoryMqServiceCallback>();

        //    Console.WriteLine("Microwave Service");
        //    Console.WriteLine("Let's prepare us some {0}", meal);

        //    _counter = 999;
        //    _timer = new Timer(BasicAck, null, 500, 500);
        //}

        //public void BasicAck(Object stateInfo)
        //{
        //    if (_counter <= 0)
        //    {
        //        _callbackChannel.UpdateStatus("* Ping *");
        //        _callbackChannel.UpdateStatus("Bon appÃ©tit");
        //        _timer.Dispose();
        //    }
        //    else
        //    {
        //        _callbackChannel.UpdateStatus(_counter.ToString());
        //        _counter--;
        //    }
        //}
        
    }
}