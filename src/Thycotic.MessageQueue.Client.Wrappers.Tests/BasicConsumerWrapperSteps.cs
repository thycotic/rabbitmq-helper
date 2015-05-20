using System;
using System.Net;
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
        [Given(@"there exists a basic consumer factory function stored in the scenario as (\w+)")]
        public void GivenThereExistsABasicConsumerFactoryFunctionStoredInTheScenario(string consumerFactoryFunctionName)
        {
            var func = new Func<Owned<IBasicConsumer<BasicConsumableDummy>>>(
                () =>
                {
                    var consumer = this.GetScenarioContext().GetSubstituteFor<IBasicConsumer<BasicConsumableDummy>>();
                    var lifetime = this.GetScenarioContext();
                    return new Owned<IBasicConsumer<BasicConsumableDummy>>(consumer, lifetime);
                });

            this.GetScenarioContext().Set(consumerFactoryFunctionName, func);
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
    }
}
