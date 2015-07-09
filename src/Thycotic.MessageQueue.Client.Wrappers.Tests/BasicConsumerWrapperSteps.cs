//Feature: BasicConsumerWrapper
	

//Background: 
//    Given there exists a substitute object for ICommonConnection stored in the scenario as CommonConnectionTest
//    And there exists a substitute object for IExchangeNameProvider stored in the scenario as ExchangeNameProviderTest
//    And there exists a substitute object for IBasicConsumer<BasicConsumableDummy> stored in the scenario as BasicConsumerTest
//    And there exists a basic consumer factory function stored in the scenario as ConsumerFactoryTest which returns Owned<IBasicConsumer<BasicConsumableDummy>> of IBasicConsumer<BasicConsumableDummy> BasicConsumerTest
//    And there exists a substitute object for IObjectSerializer stored in the scenario as ObjectSerializerTest
//    And there exists a substitute object for IMessageEncryptor stored in the scenario as MessageEncryptorTest
//    And there exists a BasicConsumableDummy stored in the scenario as BasicConsumableDummyTest
//    And the scenario object BasicConsumableDummy BasicConsumableDummyTest is not expired	
//    And the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns BasicConsumableDummy BasicConsumableDummyTest
//    And there exists a BasicConsumerWrapperDummy stored in the scenario as BasicConsumerWrapperDummyTest with CommonConnection CommonConnectionTest, ExchangeNameProvider ExchangeNameProviderTest, ConsumerFactory ConsumerFactoryTest, ObjectSerializer ObjectSerializerTest and MessageEncryptor MessageEncryptorTest

//Scenario: HandleBasicDeliver should relay message
//    Given the scenario object BasicConsumableDummy BasicConsumableDummyTest is not expired	
//    When the connection is established on ICommonConnection CommonConnectionTest
//    When the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
//    Then the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is called
//    Then the method BasicAck on the CommonModel of BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	
//Scenario: HandleBasicDeliver should not relay expired message
//    Given the scenario object BasicConsumableDummy BasicConsumableDummyTest is expired
//    And the scenario object BasicConsumableDummy BasicConsumableDummyTest should not be relayed if it is expired
//    When the connection is established on ICommonConnection CommonConnectionTest
//    When the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
//    Then the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is not called
//    Then the method BasicNack on the CommonModel of BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	
//Scenario: HandleBasicDeliver should throw away non parsable message
//    Given the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns corrupted BasicConsumableDummy message
//    When the connection is established on ICommonConnection CommonConnectionTest
//    When the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
//    Then the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is not called
//    Then the method BasicNack on the CommonModel of BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	



//using System;
//using System.Runtime.Serialization;
//using System.Threading.Tasks;
//using Autofac.Features.OwnedInstances;
//using FluentAssertions;
//using NSubstitute;
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq;
//using Thycotic.MessageQueue.Client.QueueClient;
//using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
//using Thycotic.Messages.Common;
//using Thycotic.Utility.Serialization;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MessageQueue.Client.Wrappers.Tests
//{
//    [Binding]
//    public class BasicConsumerWrapperSteps
//    {
//        [Given(@"there exists a substitute object for IBasicConsumer<BasicConsumableDummy> stored in the scenario as (\w+)")]
//        public void GivenThereExistsASubstituteObjectForIBasicConsumerBasicConsumableDummyStoredInTheScenario(string basicConsumerName)
//        {
//            this.GetScenarioContext().SetSubstitute<IBasicConsumer<BasicConsumableDummy>>(basicConsumerName);
//        }


//        [Given(@"there exists a BasicConsumableDummy stored in the scenario as (\w+)")]
//        public void GivenThereExistsABasicConsumableDummyStoredInTheScenario(string basicConsumableName)
//        {
//            this.GetScenarioContext().Set(basicConsumableName, new BasicConsumableDummy());
//        }

//        [Given(@"there exists a basic consumer factory function stored in the scenario as (\w+) which returns Owned<IBasicConsumer<BasicConsumableDummy>> of IBasicConsumer<BasicConsumableDummy> (\w+)")]
//        public void GivenThereExistsABasicConsumerFactoryFunctionStoredInTheScenario(string consumerFactoryFunctionName, string basicConsumerName)
//        {
//            var context = this.GetScenarioContext();
//            var consumer = context.Get<IBasicConsumer<BasicConsumableDummy>>(basicConsumerName);
//            Func<Owned<IBasicConsumer<BasicConsumableDummy>>> func =
//                () => new LeakyOwned<IBasicConsumer<BasicConsumableDummy>>(consumer, new LifetimeDummy());
//            this.GetScenarioContext().Set(consumerFactoryFunctionName, func);
//        }

//        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns BasicConsumableDummy (\w+)")]
//        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturns(string objectSerializerName, string consumableName)
//        {
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

//            objectSerializer.ToObject<BasicConsumableDummy>(Arg.Any<byte[]>()).Returns(this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName));
//        }
//        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns corrupted BasicConsumableDummy message")]
//        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturnsCorruptedMessage(string objectSerializerName)
//        {
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

//            objectSerializer.When(s => s.ToObject<BasicConsumableDummy>(Arg.Any<byte[]>())).Throw<SerializationException>();
//        }


//        [Given(
//            @"there exists a BasicConsumerWrapperDummy stored in the scenario as (\w+) with CommonConnection (\w+), ExchangeNameProvider (\w+), ConsumerFactory (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)"
//            )]
//        public void
//            GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenario(
//            string consumerWrapperName, string connectionName, string exchangeProviderName, string consumerFactoryName,
//            string objectSerializerName, string messageEncryptorName)
//        {
//            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
//            var exchangeNameProvider = this.GetScenarioContext().Get<IExchangeNameProvider>(exchangeProviderName);
//            var consumerFactory = this.GetScenarioContext().Get<Func<Owned<IBasicConsumer<BasicConsumableDummy>>>>(consumerFactoryName);
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
//            var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);

//            this.GetScenarioContext()
//                .Set(consumerWrapperName,
//                    new BasicConsumerWrapperDummy(connection, exchangeNameProvider, objectSerializer, messageEncryptor,
//                        consumerFactory));
//        }

//        [Given(@"the scenario object BasicConsumableDummy (\w+) is not expired")]
//        public void GivenTheScenarioObjectBasicConsumableDummyIsNotExpired(string consumableName)
//        {
//            var consumable = this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName);
//            consumable.ExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(30);
//        }

//        [Given(@"the scenario object BasicConsumableDummy (\w+) is expired")]
//        public void GivenTheScenarioObjectBasicConsumableDummyIsExpired(string consumableName)
//        {
//            var consumable = this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName);
//            consumable.ExpiresOn = DateTime.UtcNow;
//        }

//        [Given(@"the scenario object BasicConsumableDummy (\w+) should not be relayed if it is expired")]
//        public void GivenTheScenarioObjectBasicConsumableDummyShouldNotBeRedeliveredIfItIsExpired(string consumableName)
//        {
//            var consumable = this.GetScenarioContext().Get<BasicConsumableDummy>(consumableName);
//            consumable.RelayEvenIfExpired = false;
//        }

//        [When(@"the method HandleBasicDeliver on BasicConsumerWrapperDummy (\w+) is called")]
//        public void WhenTheMethodHandleBasicDeliverOnBasicConsumerWrapperDummyIsCalled(string consumerWrapperName)
//        {
//            var consumerWrapper = this.GetScenarioContext().Get<BasicConsumerWrapperDummy>(consumerWrapperName);

//            var consumerTag = Guid.NewGuid().ToString();
//            const ulong deliveryTag = 1;
//            const bool redelivered = false;
//            var exchange = Guid.NewGuid().ToString();
//            var routingKey = Guid.NewGuid().ToString();
//            var body = new byte[10];
//            var properties = new MemoryMqModelProperties(new MemoryMqProperties());

//            consumerWrapper.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties,
//                body);

//            consumerWrapper.HandleTask.Wait(TimeSpan.FromSeconds(15));

//            consumerWrapper.HandleTask.IsCompleted.Should().BeTrue("Handle task should be completed");
//        }

//        [Then(@"the method ToObject on IObjectSerializer substitute (\w+) is called")]
//        public void ThenTheMethodToObjectOnIObjectSerializerIsCalled(string objectSerializerName)
//        {
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

//            objectSerializer.Received().ToObject<BasicConsumableDummy>(Arg.Any<byte[]>());
//        }

//        [Then(@"the method Consume on IBasicConsumer<BasicConsumableDummy> (\w+) is called")]
//        public void ThenTheMethodConsumeOnIBasicConsumerIsCalled(string consumerName)
//        {
//            var consumer = this.GetScenarioContext().Get<IBasicConsumer<BasicConsumableDummy>>(consumerName);

//            consumer.Received().Consume(Arg.Any<BasicConsumableDummy>());
//        }

//        [Then(@"the method Consume on IBasicConsumer<BasicConsumableDummy> (\w+) is not called")]
//        public void ThenTheMethodConsumeOnIBasicConsumerIsNotCalled(string consumerName)
//        {
//            var consumer = this.GetScenarioContext().Get<IBasicConsumer<BasicConsumableDummy>>(consumerName);

//            consumer.DidNotReceive().Consume(Arg.Any<BasicConsumableDummy>());
//        }


//        [Then(@"the method BasicAck on the CommonModel of BasicConsumerWrapperDummy (\w+) is called")]
//        public void ThenTheMethodBasicAckOnTheCommonModelOfBasicConsumerWrapperDummyIsCalled(string consumerWrapperName)
//        {
//            var consumerWrapper = this.GetScenarioContext().Get<BasicConsumerWrapperDummy>(consumerWrapperName);
//            consumerWrapper.CommonModel.Received().BasicAck(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>());
//        }


//        [Then(@"the method BasicNack on the CommonModel of BasicConsumerWrapperDummy (\w+) is called")]
//        public void ThenTheMethodBasicNackOnTheCommonModelOfBasicConsumerWrapperDummyIsCalled(string consumerWrapperName)
//        {
//            var consumerWrapper = this.GetScenarioContext().Get<BasicConsumerWrapperDummy>(consumerWrapperName);
//            consumerWrapper.CommonModel.Received().BasicNack(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
//        }

//    }
//}
