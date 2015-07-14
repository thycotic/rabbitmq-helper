using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Testing.TestChain;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
   
    [Obsolete]
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
            return TestedSubstitute.For<ICommonModel>();
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
