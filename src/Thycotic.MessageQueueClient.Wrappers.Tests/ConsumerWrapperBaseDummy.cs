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
        public int RetryCount { get; set; }
    }

    internal class ConsumerWrapperBaseDummy : ConsumerWrapperBase<RequestDummy, object>
    {
        public ConsumerWrapperBaseDummy(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider)
            : base(connection, exchangeNameProvider)
        {

        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, ICommonModelProperties properties,
            byte[] body)
        {

        }
    }
}
