using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{


    internal class BlockingConsumerWrapperDummy : BlockingConsumerWrapper<BlockingConsumableDummy, object, IBlockingConsumer<BlockingConsumableDummy, object>>
    {
        public Task HandleTask { get; set; }

        public BlockingConsumerWrapperDummy(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<IBlockingConsumer<BlockingConsumableDummy, object>>> consumerFactory)
            : base(connection, exchangeNameProvider, objectSerializer, messageEncryptor, consumerFactory)
        {

        }

        protected override Task StartHandleTask(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
           ICommonModelProperties properties, byte[] body)
        {
            var task = base.StartHandleTask(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
            HandleTask = task;
            return task;
        }


    }
}
