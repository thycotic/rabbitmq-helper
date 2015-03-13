using FluentAssertions;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class ExchangeDictionarySteps
    {
        [Given(@"there exists a ExchangeDictionary stored in the scenario as (\w+)")]
        public void GivenThereExistsAnExchangeDictionaryStoredInTheScenario(string exchangeDictionaryName)
        {
            ScenarioContext.Current.Set(exchangeDictionaryName, new ExchangeDictionary());
        }


        [Given(@"the scenario object IExchangeDictionary (\w+) is empty")]
        public void GivenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = ScenarioContext.Current.Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }

        [When(@"the method Publish on IExchangeDictionary (\w+) is called with routing slip (\w+) and message delivery arguments (\w+)")]
        public void WhenTheMethodPublishOnExchangeDictionaryIsCalledWithRoutingSlipAndMessageDeliveryArguments(string exchangeDictionaryName, string routingSlipName, string deliveryArgumentsName)
        {
            var exchangeDictionary = ScenarioContext.Current.Get<IExchangeDictionary>(exchangeDictionaryName);
            var routingSlip = ScenarioContext.Current.Get<RoutingSlip>(routingSlipName);
            var messageDeliveryArguments = ScenarioContext.Current.Get<MemoryMqDeliveryEventArgs>(deliveryArgumentsName);

            exchangeDictionary.Publish(routingSlip, messageDeliveryArguments);
        }


        [Then(@"the scenario object IExchangeDictionary (\w+) is empty")]
        public void ThenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = ScenarioContext.Current.Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }
    }
}
