using System;
using System.Threading.Tasks;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    internal class RequestDummy : IConsumable
    {
        public RequestDummy(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }
    }

    internal class ConsumerWrapperBaseDummy : ConsumerWrapperBase<RequestDummy, object>
    {
        public ConsumerWrapperBaseDummy(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider)
            : base(connection, exchangeNameProvider)
        {

        }

        protected override Task StartHandleTask(string consumerTag, DeliveryTagWrapper deliveryTag, bool redelivered, string exchange, string routingKey,
            ICommonModelProperties properties, byte[] body)
        {
            return Task.Delay(TimeSpan.FromMilliseconds(0));
        }
    }
}
