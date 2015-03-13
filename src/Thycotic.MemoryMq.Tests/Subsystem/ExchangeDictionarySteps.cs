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
            this.GetScenarioContext().Set(exchangeDictionaryName, new ExchangeDictionary());
        }

        [Given(@"there exists a substitute object for ExchangeDictionary stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForExchangeDictionaryStoredInTheScenario(string exchangeDictionaryName)
        {
            this.GetScenarioContext().SetSubstitute<IExchangeDictionary>(exchangeDictionaryName);
        }


        [Given(@"the scenario object IExchangeDictionary (\w+) is empty")]
        public void GivenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }

        [When(@"the method Publish on IExchangeDictionary (\w+) is called with routing slip (\w+) and message delivery arguments (\w+)")]
        public void WhenTheMethodPublishOnExchangeDictionaryIsCalledWithRoutingSlipAndMessageDeliveryArguments(string exchangeDictionaryName, string routingSlipName, string deliveryArgumentsName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);
            var messageDeliveryArguments = this.GetScenarioContext().Get<MemoryMqDeliveryEventArgs>(deliveryArgumentsName);

            exchangeDictionary.Publish(routingSlip, messageDeliveryArguments);
        }


        [Then(@"the scenario object IExchangeDictionary (\w+) is empty")]
        public void ThenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }
    }
}
