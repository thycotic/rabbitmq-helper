using System;
using Autofac.Features.OwnedInstances;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{


    internal class BlockingConsumerWrapperDummy : BlockingConsumerWrapper<BlockingConsumableDummy, object, IBlockingConsumer<BlockingConsumableDummy, object>>
    {
        public BlockingConsumerWrapperDummy(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<IBlockingConsumer<BlockingConsumableDummy, object>>> consumerFactory)
            : base(connection, exchangeNameProvider, objectSerializer, messageEncryptor, consumerFactory)
        {

        }

      
    }
}
