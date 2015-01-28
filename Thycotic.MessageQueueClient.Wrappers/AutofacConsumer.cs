using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Thycotic.Logging;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public class AutofacConsumer<TMsg, THandler> : ConsumerWrapperBase<TMsg, THandler>
        where THandler : IConsumer<TMsg>
    {
        //private readonly Func<Owned<THandler>> _handlerFactory;
        private readonly IMessageSerializer _serializer;
        //private readonly IActivityMonitor _monitor;
        //private readonly IServiceBus _serviceBus;
        private readonly ILogWriter _log = Log.Get(typeof (AutofacConsumer<TMsg, THandler>));
        private readonly int _maxTries = 1;

        public AutofacConsumer(IMessageSerializer serializer,
            IRabbitMqConnection rmq) //, IServiceBus serviceBus)
            : base(rmq)
        {
            //_handlerFactory = handlerFactory;
            _serializer = serializer;
            //_monitor = monitor;
            //_serviceBus = serviceBus;
            //_maxTries = eventHandlerConfigProvider.GetMaxTries<THandler, TMsg>();
        }


        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey, IBasicProperties properties, byte[] body)
        {
            Task.Run(() => ExecuteMessage(deliveryTag, body));
        }

        public void ExecuteMessage(ulong deliveryTag, byte[] body)
        {
            var triesLeft = _maxTries;
            var success = false;

            do
            {
                --triesLeft;
                try
                {
                    //_monitor.Enter<TMsg>();
                    //var message = _serializer.BytesToMessage<TMsg>(body);

                    //using (var handler = _handlerFactory())
                    //{
                    //    handler.Value.Consume(message);
                    //}

                    //success = true;
                    //triesLeft = 0;
                }
                catch (Exception e)
                {
                    _log.Error("Failed to handle message " + typeof (TMsg).Name, e);

                    success = false;
                }
                finally
                {
                    //_monitor.Quit<TMsg>();
                }
            } while (triesLeft > 0);

            //ack/nack the message
            //TODO ??
            if (success) lock (Model) Model.BasicAck(deliveryTag, multiple: false);
            else lock (Model) Model.BasicNack(deliveryTag, multiple: false, requeue: false);
        }

     
    }
}
