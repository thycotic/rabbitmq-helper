using FluentAssertions;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class ExchangeDictionarySteps
    {
        private ExchangeDictionary _exchangeDictionary;

        [Given(@"there exists an ExchangeDictionary")]
        public void GivenThereExistsAnExchangeDictionary()
        {
            _exchangeDictionary = new ExchangeDictionary();
        }


        [Given(@"the scenario object ExchangeDictionary (\w+) is empty")]
        public void GivenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = (ExchangeDictionary)ScenarioContext.Current[exchangeDictionaryName];

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }

        [When(@"the method Publish on ExchangeDictionary (\w+) is called with routing slip (\w+) and message delivery arguments (\w+)")]
        public void WhenTheMethodPublishOnExchangeDictionaryIsCalledWithRoutingSlipAndMessageDeliveryArguments(string exchangeDictionaryName, string routingSlipName, string deliveryArgumentsName)
        {
            var exchangeDictionary = (ExchangeDictionary)ScenarioContext.Current[exchangeDictionaryName];
            var routingSlip = (RoutingSlip)ScenarioContext.Current[routingSlipName];
            var messageDeliveryArguments = (MemoryMqDeliveryEventArgs)ScenarioContext.Current[deliveryArgumentsName];

            exchangeDictionary.Publish(routingSlip, messageDeliveryArguments);
        }


        [Then(@"the scenario object ExchangeDictionary (\w+) is empty")]
        public void ThenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = (ExchangeDictionary)ScenarioContext.Current[exchangeDictionaryName];

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }
    }
}
