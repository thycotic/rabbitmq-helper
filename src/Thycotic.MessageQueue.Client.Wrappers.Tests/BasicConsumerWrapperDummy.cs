using System;
using System.Security.Policy;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using NSubstitute;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
   

    public class BasicConsumerWrapperDummy : BasicConsumerWrapper<BasicConsumableDummy, IBasicConsumer<BasicConsumableDummy>>
    {
        public Task HandleTask { get; set; }

        public BasicConsumerWrapperDummy(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<IBasicConsumer<BasicConsumableDummy>>> consumerFactory)
            : base(connection, exchangeNameProvider, objectSerializer, messageEncryptor, consumerFactory)
        {
            
        }

        protected override ICommonModel CreateModel()
        {
            return Substitute.For<ICommonModel>();
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
