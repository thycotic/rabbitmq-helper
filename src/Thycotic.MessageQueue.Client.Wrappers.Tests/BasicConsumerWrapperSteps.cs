using System;
using System.Runtime.Serialization;
using Autofac.Features.OwnedInstances;
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
    public class BasicConsumerWrapperSteps
    {

        [Given(@"there exists a substitute object for IBasicConsumer<BasicConsumableDummy> stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForICommonConnectionStoredInTheScenario(string basicConsumerName)
        {
            this.GetScenarioContext().SetSubstitute<IBasicConsumer<BasicConsumableDummy>>(basicConsumerName);
        }
 
        [Given(@"there exists a BasicConsumableDummy stored in the scenario as (\w+)")]
        public void GivenThereExistsABasicConsumableDummyStoredInTheScenario(string basicConsumableName)
        {
            this.GetScenarioContext().Set(basicConsumableName, new BasicConsumableDummy());
        }

        [Given(@"there exists a basic consumer factory function stored in the scenario as (\w+) which returns IBasicConsumer<BasicConsumableDummy> (\w+)")]
        public void GivenThereExistsABasicConsumerFactoryFunctionStoredInTheScenario(string consumerFactoryFunctionName, string basicConsumerName)
        {
            var context = this.GetScenarioContext();
            var consumer = context.Get<IBasicConsumer<BasicConsumableDummy>>(basicConsumerName);
            var lifetime = context.GetSubstituteFor<IDisposable>();
            var owned = new Owned<IBasicConsumer<BasicConsumableDummy>>(consumer, lifetime);
            
            this.GetScenarioContext().Set<Func<Owned<IBasicConsumer<BasicConsumableDummy>>>>(consumerFactoryFunctionName, () => owned);
        }

        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns (\w+)")]
        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturns(string objectSerializerName, string consumableName)
        {
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

            objectSerializer.ToObject<BasicConsumableDummy>(Arg.Any<byte[]>()).Returns(this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName));
        }

        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns corrupted message")]
        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturnsCorruptedMessage(string objectSerializerName)
        {
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

            objectSerializer.When(s => s.ToObject<BasicConsumableDummy>(Arg.Any<byte[]>())).Throw<SerializationException>();
        }

        [Given(
            @"there exists a BasicConsumerWrapperDummy stored in the scenario as (\w+) with CommonConnection (\w+), ExchangeNameProvider (\w+), ConsumerFactory (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)"
            )]
        public void
            GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenario(
            string consumerWrapperName, string connectionName, string exchangeProviderName, string consumerFactoryName,
            string objectSerializerName, string messageEncryptorName)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
            var exchangeNameProvider = this.GetScenarioContext().Get<IExchangeNameProvider>(exchangeProviderName);
            var consumerFactory = this.GetScenarioContext().Get<Func<Owned<IBasicConsumer<BasicConsumableDummy>>>>(consumerFactoryName);
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
            var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);

            this.GetScenarioContext()
                .Set(consumerWrapperName,
                    new BasicConsumerWrapperDummy(connection, exchangeNameProvider, objectSerializer, messageEncryptor,
                        consumerFactory));
        }

        [Given(@"the scenario object BasicConsumableDummy (\w+) is not expired")]
        public void GivenTheScenarioObjectBasicConsumableDummyTestIsNotExpired(string consumableName)
        {
            var consumable = this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName);
            consumable.ExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        [Given(@"the scenario object BasicConsumableDummy (\w+) is expired")]
        public void GivenTheScenarioObjectBasicConsumableDummyTestIsExpired(string consumableName)
        {
            var consumable = this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName);
            consumable.ExpiresOn = DateTime.UtcNow;
        }

        [Given(@"the scenario object BasicConsumableDummy (\w+) should not be relayed if it is expired")]
        public void GivenTheScenarioObjectBasicConsumableDummyTestShouldNotBeRedeliveredIfItIsExpired(string consumableName)
        {
            var consumable = this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName);
            consumable.RelayEvenIfExpired = false;
        }

        [When(@"the method HandleBasicDeliver on BasicConsumerWrapperDummy (\w+) is called")]
        public void WhenTheMethodHandleBasicDeliverOnBasicConsumerWrapperDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = this.GetScenarioContext().Get<BasicConsumerWrapperDummy>(consumerWrapperName);

            var consumerTag = Guid.NewGuid().ToString();
            const ulong deliveryTag = 1;
            const bool redelivered = false;
            var exchange = Guid.NewGuid().ToString();
            var routingKey = Guid.NewGuid().ToString();
            var body = new byte[10];
            var properties = new MemoryMqModelProperties(new MemoryMqProperties());

            consumerWrapper.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties,
                body);
        }

        [Then(@"the method ToObject on IObjectSerializer substitute (\w+) is called")]
        public void ThenTheMethodToObjectOnIObjectSerializerIsCalled(string objectSerializerName)
        {
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

            objectSerializer.Received().ToObject<BasicConsumableDummy>(Arg.Any<byte[]>());
        }

        [Then(@"the method Consume on IBasicConsumer<BasicConsumableDummy> (\w+) is called")]
        public void ThenTheMethodConsumeOnIBasicConsumerBasicConsumerTestIsCalled(string consumerName)
        {
            var consumer = this.GetScenarioContext().Get<IBasicConsumer<BasicConsumableDummy>>(consumerName);

            consumer.Received().Consume(Arg.Any<BasicConsumableDummy>());
        }

        [Then(@"the method Consume on IBasicConsumer<BasicConsumableDummy> (\w+) is not called")]
        public void ThenTheMethodConsumeOnIBasicConsumerBasicConsumerTestIsNotCalled(string consumerName)
        {
            var consumer = this.GetScenarioContext().Get<IBasicConsumer<BasicConsumableDummy>>(consumerName);

            consumer.DidNotReceive().Consume(Arg.Any<BasicConsumableDummy>());
        }


        [Then(@"the method BasicAck on the CommonModel of BasicConsumerWrapperDummy (\w+) is called")]
        public void ThenTheMethodBasicAckOnTheCommonModelOfBasicConsumerWrapperDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = this.GetScenarioContext().Get<BasicConsumerWrapperDummy>(consumerWrapperName);
            consumerWrapper.CommonModel.Received().BasicAck(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>());
        }


        [Then(@"the method BasicNack on the CommonModel of BasicConsumerWrapperDummy (\w+) is called")]
        public void ThenTheMethodBasicNackOnTheCommonModelOfBasicConsumerWrapperDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = this.GetScenarioContext().Get<BasicConsumerWrapperDummy>(consumerWrapperName);
            consumerWrapper.CommonModel.Received().BasicNack(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
        }

    }
}
