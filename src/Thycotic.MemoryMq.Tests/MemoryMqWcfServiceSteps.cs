using System;
using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests
{
    [Binding]
    public class MemoryMqWcfServiceSteps
    {
        [Given(@"there exists a MemoryMqWcfService stored in the scenario as (\w+) with ExchangeDictionary (\w+), BindingDictionary (\w+), ClientDictionary (\w+) and MessageDispatcher (\w+)")]
        public void GivenThereExistsAMemoryMqServerStoredInTheScenario(string serverName, string exchangesName, string bindingsName, string clientsName, string dispatcherName)
        {
            var exchange = this.GetScenarioContext().Get<IExchangeDictionary>(exchangesName);
            var bindings = this.GetScenarioContext().Get<IBindingDictionary>(bindingsName);
            var clients = this.GetScenarioContext().Get<IClientDictionary>(clientsName);
            var messageDispatcher = this.GetScenarioContext().Get<IMessageDispatcher>(dispatcherName);

            this.GetScenarioContext().Set(serverName, new MemoryMqWcfService(exchange, bindings, clients, messageDispatcher));
        }

        [When(@"the method BasicPublish on MemoryMqWcfService (\w+) is called with exchange (\w+) and routing key (\w+)")]
        public void WhenTheMethodBasicPublishOnMemoryMqServerMemoryMqServerTestIsCalled(string serverName, string exchangeName, string routingKey)
        {
            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);

            server.BasicPublish(exchangeName, routingKey, true, true, new MemoryMqProperties(), null);
        }

        [When(@"the method QueueBind on MemoryMqWcfService (\w+) is called")]
        public void WhenTheMethodQueueBindOnMemoryMqServerMemoryMqServerTestIsCalled(string serverName)
        {
            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
            server.QueueBind(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [When(@"the method BasicConsume on MemoryMqWcfService (\w+) is called")]
        public void WhenTheMethodBasicConsumeOnMemoryMqServerIsCalled(string serverName)
        {
            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
            var queueName = Guid.NewGuid().ToString();
            server.BasicConsume(queueName);
        }


        [When(@"the method BasicAck on MemoryMqWcfService (\w+) is called")]
        public void WhenTheMethodBasicAckOnMemoryMqServerIsCalled(string serverName)
        {
            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
            const ulong deliveryTag = 0;
            var exchange = Guid.NewGuid().ToString();
            var routingKey = Guid.NewGuid().ToString();
            server.BasicAck(deliveryTag, exchange, routingKey, false);
        }

        [When(@"the method BasicNack on MemoryMqWcfService (\w+) is called")]
        public void WhenTheMethodBasicNackOnMemoryMqServerIsCalled(string serverName)
        {
            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
            const ulong deliveryTag = 0;
            var exchange = Guid.NewGuid().ToString();
            var routingKey = Guid.NewGuid().ToString();
            server.BasicNack(deliveryTag, exchange, routingKey, false);
        }

        [When(@"the method Dispose on MemoryMqWcfService (\w+) is called")]
        public void WhenTheMethodDisposeOnMemoryMqServerIsCalled(string serverName)
        {
            var server = this.GetScenarioContext().Get<IMemoryMqWcfService>(serverName);
            server.Dispose();
        }

        [Then(@"the method Start on MessageDispatcher substitute (\w+) is called")]
        public void ThenTheMethodStartOnMessageDispatcherSubstituteIsCalled(string dispatcherName)
        {
            var messageDispatcher = this.GetScenarioContext().Get<IMessageDispatcher>(dispatcherName);

            messageDispatcher.Received().Start();       
        }

        [Then(@"the method Publish on ExchangeDictionary substitute (\w+) is called with exchange (\w+) and routing key (\w+)")]
        public void ThenTheMethodexchangesNameOnExchangeDictionarySubstituteIsCalled(string exchangesName, string exchangeName, string routingKey)
        {
            var exchange = this.GetScenarioContext().Get<IExchangeDictionary>(exchangesName);

            exchange.Received().Publish(new RoutingSlip(exchangeName, routingKey), Arg.Any<MemoryMqDeliveryEventArgs>());
        }

        [Then(@"the method AddBinding on BindingDictionary substitute (\w+) is called")]
        public void ThenTheMethodAddBindingOnBindingDictionarySubstituteIsCalled(string bindingsName)
        {
            var bindings = this.GetScenarioContext().Get<IBindingDictionary>(bindingsName);
            bindings.Received().AddBinding(Arg.Any<RoutingSlip>(), Arg.Any<string>());
        }

        [Then(@"the method AddClient on ClientDictionary substitute (\w+) is called")]
        public void ThenTheMethodAddClientOnClientDictionarySubstituteIsCalled(string clientsName)
        {
            var clients = this.GetScenarioContext().Get<IClientDictionary>(clientsName);
            clients.Received().AddClient(Arg.Any<string>());
        }

        [Then(@"the method Acknowledge on ExchangeDictionary substitute (\w+) is called")]
        public void ThenTheMethodAcknowledgeOnExchangeDictionaryIsCalled(string exchangeName)
        {
            var exchange = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeName);
            exchange.Received().Acknowledge(Arg.Any<ulong>(), Arg.Any<RoutingSlip>());
        }

        [Then(@"the method NegativelyAcknowledge on ExchangeDictionary substitute (\w+) is called")]
        public void ThenTheMethodNegativelyAcknowledgeOnExchangeDictionarySubstituteIsCalled(string exchangeName)
        {
            var messages = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeName);
            messages.Received().NegativelyAcknowledge(Arg.Any<ulong>(), Arg.Any<RoutingSlip>());
        }

        [Then(@"the method Dispose on MessageDispatcher substitute (\w+) is called")]
        public void ThenTheMethodStopOnMessageDispatcherSubstituteIsCalled(string dispatcherName)
        {
            var messageDispatcher = this.GetScenarioContext().Get<IMessageDispatcher>(dispatcherName);
            messageDispatcher.Received().Dispose();
        }
    }
}
