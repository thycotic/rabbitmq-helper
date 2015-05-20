using System;
using Autofac.Features.OwnedInstances;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
   

    internal class BasicConsumerWrapperDummy : BasicConsumerWrapper<BasicConsumableDummy, IBasicConsumer<BasicConsumableDummy>>
    {
        public BasicConsumerWrapperDummy(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<IBasicConsumer<BasicConsumableDummy>>> consumerFactory)
            : base(connection, exchangeNameProvider, objectSerializer, messageEncryptor, consumerFactory)
        {

        }

      
    }
}
