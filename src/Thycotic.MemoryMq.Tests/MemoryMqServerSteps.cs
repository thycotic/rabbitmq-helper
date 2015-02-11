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

        [When(@"the method BasicPublish on MemoryMqServer (\w+) is called")]
        public void WhenTheMethodBasicPublishOnMemoryMqServerMemoryMqServerTestIsCalled(string serverName)
        {
            var server = (IMemoryMqServer)ScenarioContext.Current[serverName];

            server.BasicPublish(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), true, true, new MemoryMqProperties(), null);
        }


        [Then(@"the method Start on MessageDispatcher substitute (\w+) is called")]
        public void ThenTheMethodStartOnMessageDispatcherSubstituteIsCalled(string dispatcherName)
        {
            var messageDispatcher = (IMessageDispatcher)ScenarioContext.Current[dispatcherName];

            messageDispatcher.Received().Start();       
        }

        [Then(@"the method Publish on ExchangeDictionary substitute (\w+) is called")]
        public void ThenTheMethodexchangesNameOnExchangeDictionarySubstituteIsCalled(string exchangesName)
        {
            var exchange = (IExchangeDictionary)ScenarioContext.Current[exchangesName];

            exchange.Received().Publish(Arg.Any<RoutingSlip>(), Arg.Any<MemoryMqDeliveryEventArgs>());
        }
    }
}
