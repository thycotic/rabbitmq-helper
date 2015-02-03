using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel;
using Thycotic.Logging;
using Thycotic.MemoryMq.Collections;

namespace Thycotic.MemoryMq
{
    public class MemoryMqServiceClient : IMemoryMqServiceClient
    {
        private readonly ConcurrentHashSet<IContextChannel> _consumers = new ConcurrentHashSet<IContextChannel>();
        private readonly ConcurrentDictionary<string, string> _bindings = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentDictionary<string, ConcurrentQueue<byte[]>> _messages = new ConcurrentDictionary<string, ConcurrentQueue<byte[]>>();

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServiceClient));

        public void AttachConsumer()
        {
            _log.Debug("Attaching consumer");

            // ReSharper disable once SuspiciousTypeConversion.Global
            var callbackChannel = (IContextChannel)OperationContext.Current.GetCallbackChannel<IMemoryMqServiceCallback>();

            //have the consumer remove itself when it disconnects
            callbackChannel.Closed += (sender, args) =>
            {
                _log.Debug("Detaching consumer");
                _consumers.Remove(callbackChannel);
            };
            _consumers.Add(callbackChannel);
        }

        private static string GetFullRoutingSlip(string exchange, string routingKey)
        {
            return string.Format("{0}:{1}", exchange, routingKey);
        }

        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, byte[] body)
        {
            var fullRoutingSlip = GetFullRoutingSlip(exchangeName, routingKey);
            _messages.GetOrAdd(fullRoutingSlip, s => new ConcurrentQueue<byte[]>());

            _messages[fullRoutingSlip].Enqueue(body);
        }

        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            _bindings.TryAdd(queueName, GetFullRoutingSlip(exchangeName, routingKey));
        }

        public MemoryQueueDeliveryEventArgs BasicConsume(string queueName)
        {
            var fullRoutingSplip = _bindings[queueName];

            ConcurrentQueue<byte[]> queue;
            if (!_messages.TryGetValue(fullRoutingSplip, out queue))
            {
                return null;
            }

            byte[] body;
            if (queue.TryDequeue(out body))
            {
                string consumerTag = string.Empty;
                ulong deliveryTag = 1;
                bool redelivered = false;
                string exchange = string.Empty;
                string routingKey = string.Empty;
                return new MemoryQueueDeliveryEventArgs(consumerTag, deliveryTag, redelivered, exchange, routingKey, body);
            }

            return null;

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