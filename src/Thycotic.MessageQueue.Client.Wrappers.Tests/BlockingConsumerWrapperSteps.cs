using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using FluentAssertions;
using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    [Binding]
    public class BlockingConsumerWrapperSteps
    {
        [Given(@"there exists a substitute object for IBlockingConsumer<BlockingConsumableDummy, object> stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIBlockingConsumerBlockingConsumableDummyStoredInTheScenario(string blockingConsumerName)
        {
            this.GetScenarioContext().SetSubstitute<IBlockingConsumer<BlockingConsumableDummy, object>>(blockingConsumerName);
        }


        [Given(@"there exists a BlockingConsumableDummy stored in the scenario as (\w+)")]
        public void GivenThereExistsABlockingConsumableDummyStoredInTheScenario(string blockingConsumableName)
        {
            this.GetScenarioContext().Set(blockingConsumableName, new BlockingConsumableDummy());
        }

        [Given(@"there exists a blocking consumer factory function stored in the scenario as (\w+) which returns Owned<IBlockingConsumer<BlockingConsumableDummy>> of IBlockingConsumer<BlockingConsumableDummy> (\w+)")]
        public void GivenThereExistsABlockingConsumerFactoryFunctionStoredInTheScenario(string consumerFactoryFunctionName, string blockingConsumerName)
        {
            var context = this.GetScenarioContext();
            var consumer = context.Get<IBlockingConsumer<BlockingConsumableDummy, object>>(blockingConsumerName);
            Func<Owned<IBlockingConsumer<BlockingConsumableDummy, object>>> func =
                () => new LeakyOwned<IBlockingConsumer<BlockingConsumableDummy, object>>(consumer, new LifetimeDummy());
            this.GetScenarioContext().Set(consumerFactoryFunctionName, func);
        }

        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns BlockingConsumableDummy (\w+)")]
        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturns(string objectSerializerName, string consumableName)
        {
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

            objectSerializer.ToObject<BlockingConsumableDummy>(Arg.Any<byte[]>()).Returns(this.GetScenarioContext().Get<BlockingConsumableDummy>(consumableName));
        }

        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns corrupted BlockingConsumableDummy message")]
        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturnsCorruptedMessage(string objectSerializerName)
        {
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

            objectSerializer.When(s => s.ToObject<BlockingConsumableDummy>(Arg.Any<byte[]>())).Throw<SerializationException>();
        }

        [Given(
            @"there exists a BlockingConsumerWrapperDummy stored in the scenario as (\w+) with CommonConnection (\w+), ExchangeNameProvider (\w+), ConsumerFactory (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)"
            )]
        public void
            GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenario(
            string consumerWrapperName, string connectionName, string exchangeProviderName, string consumerFactoryName,
            string objectSerializerName, string messageEncryptorName)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
            var exchangeNameProvider = this.GetScenarioContext().Get<IExchangeNameProvider>(exchangeProviderName);
            var consumerFactory = this.GetScenarioContext().Get<Func<Owned<IBlockingConsumer<BlockingConsumableDummy, object>>>>(consumerFactoryName);
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
            var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);

            this.GetScenarioContext()
                .Set(consumerWrapperName,
                    new BlockingConsumerWrapperDummy(connection, exchangeNameProvider, objectSerializer, messageEncryptor,
                        consumerFactory));
        }
        
        [When(@"the method HandleBasicDeliver on BlockingConsumerWrapperDummy (\w+) is called")]
        public void WhenTheMethodHandleBlockingDeliverOnBlockingConsumerWrapperDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = this.GetScenarioContext().Get<BlockingConsumerWrapperDummy>(consumerWrapperName);

            var consumerTag = Guid.NewGuid().ToString();
            const ulong deliveryTag = 1;
            const bool redelivered = false;
            var exchange = Guid.NewGuid().ToString();
            var routingKey = Guid.NewGuid().ToString();
            var body = new byte[10];
            var properties = new MemoryMqModelProperties(new MemoryMqProperties()) {ReplyTo = Guid.NewGuid().ToString()};

            consumerWrapper.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties,
                body);

            consumerWrapper.HandleTask.Wait(TimeSpan.FromSeconds(15));

            consumerWrapper.HandleTask.IsCompleted.Should().BeTrue("Handle task should be completed");
        }

        [Then(@"the method ToObject on IObjectSerializer substitute (\w+) is called")]
        public void ThenTheMethodToObjectOnIObjectSerializerIsCalled(string objectSerializerName)
        {
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

            objectSerializer.Received().ToObject<BlockingConsumableDummy>(Arg.Any<byte[]>());
        }

        [Then(@"the method Consume on IBlockingConsumer<BlockingConsumableDummy> (\w+) is called")]
        public void ThenTheMethodConsumeOnIBlockingConsumerIsCalled(string consumerName)
        {
            var consumer = this.GetScenarioContext().Get<IBlockingConsumer<BlockingConsumableDummy, object>>(consumerName);

            consumer.Received().Consume(Arg.Any<BlockingConsumableDummy>());
        }

        [Then(@"the method Consume on IBlockingConsumer<BlockingConsumableDummy> (\w+) is not called")]
        public void ThenTheMethodConsumeOnIBlockingConsumerIsNotCalled(string consumerName)
        {
            var consumer = this.GetScenarioContext().Get<IBlockingConsumer<BlockingConsumableDummy, object>>(consumerName);

            consumer.DidNotReceive().Consume(Arg.Any<BlockingConsumableDummy>());
        }


        [Then(@"the method BlockingAck on the CommonModel of BlockingConsumerWrapperDummy (\w+) is called")]
        public void ThenTheMethodBlockingAckOnTheCommonModelOfBlockingConsumerWrapperDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = this.GetScenarioContext().Get<BlockingConsumerWrapperDummy>(consumerWrapperName);
            consumerWrapper.CommonModel.Received().BasicAck(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>());
        }


        [Then(@"the method BlockingNack on the CommonModel of BlockingConsumerWrapperDummy (\w+) is called")]
        public void ThenTheMethodBlockingNackOnTheCommonModelOfBlockingConsumerWrapperDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = this.GetScenarioContext().Get<BlockingConsumerWrapperDummy>(consumerWrapperName);
            consumerWrapper.CommonModel.Received().BasicNack(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
        }

        [Then(@"the method OpenChannel on ICommonConnection (\w+) is called")]
        public void ThenTheMethodOpenChannelOnICommonConnectionIsCalled(string connectionName)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);

            connection.Received().OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>());
        }
    }
}
