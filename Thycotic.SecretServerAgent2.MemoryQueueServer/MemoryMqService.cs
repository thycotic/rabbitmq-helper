using System;
using System.ServiceModel;
using System.Threading;

namespace Thycotic.SecretServerAgent2.MemoryQueueServer
{
    public class MemoryMqService : IMemoryMqService
    {
        private IMemoryMqCallback _callbackChannel;
        private int _counter;
        private Timer _timer;

        #region IMicrowaveService Members

        public void BasicPublish(string meal)
        {
            _callbackChannel = OperationContext.Current
                .GetCallbackChannel<IMemoryMqCallback>();

            Console.WriteLine("Microwave Service");
            Console.WriteLine("Let's prepare us some {0}", meal);

            _counter = 999;
            _timer = new Timer(BasicAck, null, 500, 500);
        }

        public void BlockingPublish(string meal)
        {
            _callbackChannel = OperationContext.Current
                .GetCallbackChannel<IMemoryMqCallback>();

            Console.WriteLine("Microwave Service");
            Console.WriteLine("Let's prepare us some {0}", meal);

            _counter = 999;
            _timer = new Timer(BasicAck, null, 500, 500);
        }

        #endregion

        public void BasicAck(Object stateInfo)
        {
            if (_counter <= 0)
            {
                _callbackChannel.UpdateStatus("* Ping *");
                _callbackChannel.UpdateStatus("Bon appÃ©tit");
                _timer.Dispose();
            }
            else
            {
                _callbackChannel.UpdateStatus(_counter.ToString());
                _counter--;
            }
        }
    }
}