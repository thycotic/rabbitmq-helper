using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel;
using Thycotic.Logging;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Exchange that binds topics to queues
    /// </summary>
    public class Exchange 
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<byte[]>> _data = new ConcurrentDictionary<string, ConcurrentQueue<byte[]>>();

        private readonly ILogWriter _log = Log.Get(typeof(Exchange));

        public void Publish(string routingSlip, byte[] body)
        {
            _data.GetOrAdd(routingSlip, s => new ConcurrentQueue<byte[]>());

            _data[routingSlip].Enqueue(body);
        }
    }

    /// <summary>
    /// Bindings which bind queues, to topics
    /// </summary>
    public class Bindings
    {
        private readonly  ConcurrentDictionary<string, string> _data= new ConcurrentDictionary<string, string>();

        private readonly ILogWriter _log = Log.Get(typeof(Bindings));

        public void AddBinding(string queueName, string routingSlip)
        {
            _data.TryAdd(queueName, routingSlip);
        }
    }

    public class Consumers
    {
        private readonly ConcurrentDictionary<string, ConsumerList> _data =
            new ConcurrentDictionary<string, ConsumerList>();

        private readonly ILogWriter _log = Log.Get(typeof(Consumers));

        public void AddConsumer(string queueName)
        {
// ReSharper disable once SuspiciousTypeConversion.Global
            var callbackChannel = (IContextChannel)OperationContext.Current.GetCallbackChannel<IMemoryMqServiceCallback>();

            //have the consumer remove itself when it disconnects
            callbackChannel.Closed += (sender, args) =>
            {
                _log.Debug("Detaching consumer");
                GetConsumerList(queueName).RemoveConsumer(callbackChannel);
            };

            GetConsumerList(queueName).AddConsumer(callbackChannel);
        }

        private ConsumerList GetConsumerList(string queueName)
        {
            return _data.GetOrAdd(queueName, s => new ConsumerList());
        }


        private class ConsumerList
        {
            private readonly ConcurrentQueue<IContextChannel> _data = new ConcurrentQueue<IContextChannel>();

            private readonly ILogWriter _log = Log.Get(typeof(ConsumerList));

            public void AddConsumer(IContextChannel consumer)
            {
                _data.Enqueue(consumer);
            }

            public void RemoveConsumer(IContextChannel consumer)
            {
                //_data.
            }
        }
    }

    public class MemoryMqServiceClient : IMemoryMqServiceClient
    {
        private readonly Exchange _messages = new Exchange();
        private readonly Bindings _bindings = new Bindings();
        private readonly Consumers _consumers = new Consumers();

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServiceClient));

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
            _bindings.AddBinding(queueName, GetFullRoutingSlip(exchangeName, routingKey));
        }

        public void BasicConsume(string queueName)
        {
            _log.Debug("Attaching consumer");
            
               _consumers.AddConsumer(queueName);


            //var fullRoutingSplip = _bindings[queueName];

            //ConcurrentQueue<byte[]> queue;
            //if (!_messages.TryGetValue(fullRoutingSplip, out queue))
            //{
            //    return null;
            //}

            //byte[] body;
            //if (queue.TryDequeue(out body))
            //{
            //    string consumerTag = string.Empty;
            //    ulong deliveryTag = 1;
            //    bool redelivered = false;
            //    string exchange = string.Empty;
            //    string routingKey = string.Empty;
            //    return new MemoryQueueDeliveryEventArgs(consumerTag, deliveryTag, redelivered, exchange, routingKey, body);
            //}

            //return null;

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