//Feature: MemoryMqWcfService

//Background: 
//    Given there exists a substitute object for IExchangeDictionary stored in the scenario as ExchangeDictionaryTest
//    And there exists a substitute object for IBindingDictionary stored in the scenario as BindingDictionaryTest
//    And there exists a substitute object for IClientDictionary stored in the scenario as ClientDictionaryTest
//    And there exists a substitute object for IMessageDispatcher stored in the scenario as MessageDispatcherTest
//    And there exists a MemoryMqWcfService stored in the scenario as MemoryMqWcfServiceTest with ExchangeDictionary ExchangeDictionaryTest, BindingDictionary BindingDictionaryTest, ClientDictionary ClientDictionaryTest and MessageDispatcher MessageDispatcherTest

//#TODO: Check for explicit parameters

//@mytag
//Scenario: Constructor calls Start on dispatcher
//    Then the method Start on IMessageDispatcher substitute MessageDispatcherTest is called

//Scenario: BasicPublish calls Publish on ExchangeDictionary
//    When the method BasicPublish on IMemoryMqWcfService MemoryMqWcfServiceTest is called with exchange ExchangeTest and routing key RoutingKeyTest
//    Then the method Publish on IExchangeDictionary substitute ExchangeDictionaryTest is called with exchange ExchangeTest and routing key RoutingKeyTest

//Scenario: QueueBind calls AddBinding on BindingDictionary
//    When the method QueueBind on IMemoryMqWcfService MemoryMqWcfServiceTest is called
//    Then the method AddBinding on IBindingDictionary substitute BindingDictionaryTest is called

//Scenario: BasicConsume calls AddClient on ClientDictionary
//    When the method BasicConsume on IMemoryMqWcfService MemoryMqWcfServiceTest is called
//    Then the method AddClient on IClientDictionary substitute ClientDictionaryTest is called

//Scenario: BasicAck calls Acknowledge on ExchangeDictionary
//    When the method BasicAck on IMemoryMqWcfService MemoryMqWcfServiceTest is called
//    Then the method Acknowledge on IExchangeDictionary substitute ExchangeDictionaryTest is called

//Scenario: BasicNack calls NegativelyAcknowledge on ExchangeDictionary
//    When the method BasicNack on IMemoryMqWcfService MemoryMqWcfServiceTest is called
//    Then the method NegativelyAcknowledge on IExchangeDictionary substitute ExchangeDictionaryTest is called

//Scenario: Dispose calls Stop on MessageDispatcher
//    When the method Dispose on IMemoryMqWcfService MemoryMqWcfServiceTest is called
//    Then the method Dispose on IMessageDispatcher substitute MessageDispatcherTest is called


//using System;
//using NSubstitute;
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq.Subsystem;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MemoryMq.Tests
//{
//    [Binding]
//    public class MemoryMqWcfServiceSteps
//    {
//        [Given(@"there exists a MemoryMqWcfService stored in the scenario as (\w+) with ExchangeDictionary (\w+), BindingDictionary (\w+), ClientDictionary (\w+) and MessageDispatcher (\w+)")]
//        public void GivenThereExistsAMemoryMqServerStoredInTheScenario(string serverName, string exchangesName, string bindingsName, string clientsName, string dispatcherName)
//        {
//            var exchange = this.GetScenarioContext().Get<IExchangeDictionary>(exchangesName);
//            var bindings = this.GetScenarioContext().Get<IBindingDictionary>(bindingsName);
//            var clients = this.GetScenarioContext().Get<IClientDictionary>(clientsName);
//            var messageDispatcher = this.GetScenarioContext().Get<IMessageDispatcher>(dispatcherName);

//            this.GetScenarioContext().Set(serverName, new MemoryMqWcfService(exchange, bindings, clients, messageDispatcher));
//        }

//        [When(@"the method BasicPublish on IMemoryMqWcfService (\w+) is called with exchange (\w+) and routing key (\w+)")]
//        public void WhenTheMethodBasicPublishOnIMemoryMqServerMemoryMqServerTestIsCalled(string serverName, string exchangeName, string routingKey)
//        {
//            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);

//            server.BasicPublish(exchangeName, routingKey, true, true, new MemoryMqProperties(), null);
//        }

//        [When(@"the method QueueBind on IMemoryMqWcfService (\w+) is called")]
//        public void WhenTheMethodQueueBindOnIMemoryMqServerMemoryMqServerTestIsCalled(string serverName)
//        {
//            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
//            server.QueueBind(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
//        }

//        [When(@"the method BasicConsume on IMemoryMqWcfService (\w+) is called")]
//        public void WhenTheMethodBasicConsumeOnIMemoryMqServerIsCalled(string serverName)
//        {
//            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
//            var queueName = Guid.NewGuid().ToString();
//            server.BasicConsume(queueName);
//        }


//        [When(@"the method BasicAck on IMemoryMqWcfService (\w+) is called")]
//        public void WhenTheMethodBasicAckOnIMemoryMqServerIsCalled(string serverName)
//        {
//            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
//            const ulong deliveryTag = 0;
//            var exchange = Guid.NewGuid().ToString();
//            var routingKey = Guid.NewGuid().ToString();
//            server.BasicAck(deliveryTag, exchange, routingKey, false);
//        }

//        [When(@"the method BasicNack on IMemoryMqWcfService (\w+) is called")]
//        public void WhenTheMethodBasicNackOnIMemoryMqServerIsCalled(string serverName)
//        {
//            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
//            const ulong deliveryTag = 0;
//            var exchange = Guid.NewGuid().ToString();
//            var routingKey = Guid.NewGuid().ToString();
//            server.BasicNack(deliveryTag, exchange, routingKey, false, false);
//        }

//        [When(@"the method Dispose on IMemoryMqWcfService (\w+) is called")]
//        public void WhenTheMethodDisposeOnIMemoryMqServerIsCalled(string serverName)
//        {
//            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
//            server.Dispose();
//        }

//        [Then(@"the method Start on IMessageDispatcher substitute (\w+) is called")]
//        public void ThenTheMethodStartOnIMessageDispatcherSubstituteIsCalled(string dispatcherName)
//        {
//            var messageDispatcher = this.GetScenarioContext().Get<IMessageDispatcher>(dispatcherName);

//            messageDispatcher.Received().Start();       
//        }

//        [Then(@"the method Publish on IExchangeDictionary substitute (\w+) is called with exchange (\w+) and routing key (\w+)")]
//        public void ThenTheMethodexchangesNameOnIExchangeDictionarySubstituteIsCalled(string exchangesName, string exchangeName, string routingKey)
//        {
//            var exchange = this.GetScenarioContext().Get<IExchangeDictionary>(exchangesName);

//            exchange.Received().Publish(new RoutingSlip(exchangeName, routingKey), Arg.Any<MemoryMqDeliveryEventArgs>());
//        }

//        [Then(@"the method AddBinding on IBindingDictionary substitute (\w+) is called")]
//        public void ThenTheMethodAddBindingOnIBindingDictionarySubstituteIsCalled(string bindingsName)
//        {
//            var bindings = this.GetScenarioContext().Get<IBindingDictionary>(bindingsName);
//            bindings.Received().AddBinding(Arg.Any<RoutingSlip>(), Arg.Any<string>());
//        }

//        [Then(@"the method AddClient on IClientDictionary substitute (\w+) is called")]
//        public void ThenTheMethodAddClientOnIClientDictionarySubstituteIsCalled(string clientsName)
//        {
//            var clients = this.GetScenarioContext().Get<IClientDictionary>(clientsName);
//            clients.Received().AddClient(Arg.Any<string>());
//        }

//        [Then(@"the method Acknowledge on IExchangeDictionary substitute (\w+) is called")]
//        public void ThenTheMethodAcknowledgeOnIExchangeDictionaryIsCalled(string exchangeName)
//        {
//            var exchange = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeName);
//            exchange.Received().Acknowledge(Arg.Any<ulong>(), Arg.Any<RoutingSlip>());
//        }

//        [Then(@"the method NegativelyAcknowledge on IExchangeDictionary substitute (\w+) is called")]
//        public void ThenTheMethodNegativelyAcknowledgeOnIExchangeDictionarySubstituteIsCalled(string exchangeName)
//        {
//            var messages = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeName);
//            messages.Received().NegativelyAcknowledge(Arg.Any<ulong>(), Arg.Any<RoutingSlip>(), Arg.Any<bool>());
//        }

//        [Then(@"the method Dispose on IMessageDispatcher substitute (\w+) is called")]
//        public void ThenTheMethodStopOnIMessageDispatcherSubstituteIsCalled(string dispatcherName)
//        {
//            var messageDispatcher = this.GetScenarioContext().Get<IMessageDispatcher>(dispatcherName);
//            messageDispatcher.Received().Dispose();
//        }
//    }
//}
