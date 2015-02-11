using System;
using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq.Tests
{
    [Binding]
    public class MemoryMqServerSteps
    {
        [Given(@"there exists a MemoryMqServer stored in the scenario as (\w+) with ExchangeDictionary (\w+), BindingDictionary (\w+), ClientDictionary (\w+) and MessageDispatcher (\w+)")]
        public void GivenThereExistsAMemoryMqServerStoredInTheScenario(string serverName, string exchangesName, string bindingsName, string clientsName, string dispatcherName)
        {
            var exchange = (IExchangeDictionary)ScenarioContext.Current[exchangesName];
            var bindings = (IBindingDictionary)ScenarioContext.Current[bindingsName];
            var clients = (IClientDictionary)ScenarioContext.Current[clientsName];
            var messageDispatcher = (IMessageDispatcher)ScenarioContext.Current[dispatcherName];

            ScenarioContext.Current[serverName] = new MemoryMqServer(exchange, bindings, clients, messageDispatcher);
        }

        [When(@"the method BasicPublish on MemoryMqServer (\w+) is called with exchange (\w+) and routing key (\w+)")]
        public void WhenTheMethodBasicPublishOnMemoryMqServerMemoryMqServerTestIsCalled(string serverName, string exchangeName, string routingKey)
        {
            var server = (IMemoryMqServer)ScenarioContext.Current[serverName];

            server.BasicPublish(exchangeName, routingKey, true, true, new MemoryMqProperties(), null);
        }

        [When(@"the method QueueBind on MemoryMqServer (\w+) is called")]
        public void WhenTheMethodQueueBindOnMemoryMqServerMemoryMqServerTestIsCalled(string serverName)
        {
            var server = (IMemoryMqServer)ScenarioContext.Current[serverName];
            server.QueueBind(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [When(@"the method BasicConsume on MemoryMqServer (\w+) is called")]
        public void WhenTheMethodBasicConsumeOnMemoryMqServerIsCalled(string serverName)
        {
            var server = (IMemoryMqServer)ScenarioContext.Current[serverName];
            var queueName = Guid.NewGuid().ToString();
            server.BasicConsume(queueName);
        }


        [When(@"the method BasicAck on MemoryMqServer (\w+) is called")]
        public void WhenTheMethodBasicAckOnMemoryMqServerIsCalled(string serverName)
        {
            var server = (IMemoryMqServer)ScenarioContext.Current[serverName];
            const ulong deliveryTag = 0;
            var exchange = Guid.NewGuid().ToString();
            var routingKey = Guid.NewGuid().ToString();
            server.BasicAck(deliveryTag, exchange, routingKey, false);
        }

        [When(@"the method BasicNack on MemoryMqServer (\w+) is called")]
        public void WhenTheMethodBasicNackOnMemoryMqServerIsCalled(string serverName)
        {
            var server = (IMemoryMqServer)ScenarioContext.Current[serverName];
            const ulong deliveryTag = 0;
            var exchange = Guid.NewGuid().ToString();
            var routingKey = Guid.NewGuid().ToString();
            server.BasicNack(deliveryTag, exchange, routingKey, false);
        }

        [When(@"the method Dispose on MemoryMqServer (\w+) is called")]
        public void WhenTheMethodDisposeOnMemoryMqServerIsCalled(string serverName)
        {
            var server = (IMemoryMqServer)ScenarioContext.Current[serverName];
            server.Dispose();
        }

        [Then(@"the method Start on MessageDispatcher substitute (\w+) is called")]
        public void ThenTheMethodStartOnMessageDispatcherSubstituteIsCalled(string dispatcherName)
        {
            var messageDispatcher = (IMessageDispatcher)ScenarioContext.Current[dispatcherName];

            messageDispatcher.Received().Start();       
        }

        [Then(@"the method Publish on ExchangeDictionary substitute (\w+) is called with exchange (\w+) and routing key (\w+)")]
        public void ThenTheMethodexchangesNameOnExchangeDictionarySubstituteIsCalled(string exchangesName, string exchangeName, string routingKey)
        {
            var exchange = (IExchangeDictionary)ScenarioContext.Current[exchangesName];

            exchange.Received().Publish(new RoutingSlip(exchangeName, routingKey), Arg.Any<MemoryMqDeliveryEventArgs>());
        }

        [Then(@"the method AddBinding on BindingDictionary substitute (\w+) is called")]
        public void ThenTheMethodAddBindingOnBindingDictionarySubstituteIsCalled(string bindingsName)
        {
            var queue = (IBindingDictionary)ScenarioContext.Current[bindingsName];
            queue.Received().AddBinding(Arg.Any<RoutingSlip>(), Arg.Any<string>());
        }

        [Then(@"the method AddClient on ClientDictionary substitute (\w+) is called")]
        public void ThenTheMethodAddClientOnClientDictionarySubstituteIsCalled(string clientsName)
        {
            var clients = (IClientDictionary)ScenarioContext.Current[clientsName];
            clients.Received().AddClient(Arg.Any<string>());
        }

        [Then(@"the method Acknowledge on ExchangeDictionary substitute (\w+) is called")]
        public void ThenTheMethodAcknowledgeOnExchangeDictionaryIsCalled(string exchangeName)
        {
            var messages = (IExchangeDictionary)ScenarioContext.Current[exchangeName];
            messages.Received().Acknowledge(Arg.Any<ulong>(), Arg.Any<RoutingSlip>());
        }

        [Then(@"the method NegativelyAcknowledge on ExchangeDictionary substitute (\w+) is called")]
        public void ThenTheMethodNegativelyAcknowledgeOnExchangeDictionarySubstituteIsCalled(string exchangeName)
        {
            var messages = (IExchangeDictionary)ScenarioContext.Current[exchangeName];
            messages.Received().NegativelyAcknowledge(Arg.Any<ulong>(), Arg.Any<RoutingSlip>());
        }

        [Then(@"the method Stop on MessageDispatcher substitute (\w+) is called")]
        public void ThenTheMethodStopOnMessageDispatcherSubstituteIsCalled(string dispatcherName)
        {
            var messageDispatcher = (IMessageDispatcher)ScenarioContext.Current[dispatcherName];
            messageDispatcher.Received().Stop();
        }
    }
}
