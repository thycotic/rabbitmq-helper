using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace Thycotic.MemoryMq
{
    public class MemoryMqServiceClient : IMemoryMqServiceClient
    {
        private ConcurrentDictionary<string, ConcurrentQueue<byte[]>> _messages = new ConcurrentDictionary<string, ConcurrentQueue<byte[]>>();
        
        private IMemoryMqServiceCallback _callbackChannel;
        private int _counter;
        private Timer _timer;

        private static string GetFullRoutingSlip(string exchange, string routingKey)
        {
            return string.Format("{0}:{1}", exchange, routingKey);
        }

        public void ConfirmSelect()
        {
            
        }

        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, byte[] body)
        {
            var fullRoutingSlip = GetFullRoutingSlip(exchangeName, routingKey);
            _messages.GetOrAdd(fullRoutingSlip, s => new ConcurrentQueue<byte[]>());

            _messages[fullRoutingSlip].Enqueue(body);
        }

        public void WaitForConfirmsOrDie(TimeSpan confirmationTimeout)
        {
            
        }

        public void ExchangeDeclare(string exchangeName, string exchangeType)
        {
            
        }

        public void QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete)
        {
            
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

    internal class MemoryMqHash
    {
    }
}