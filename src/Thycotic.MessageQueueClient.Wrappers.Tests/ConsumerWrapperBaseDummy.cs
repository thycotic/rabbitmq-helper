using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers.Tests
{
    internal class RequestDummy : IConsumable
    {
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
