using FluentAssertions;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class ExchangeDictionarySteps
    {
        [Given(@"the scenario object ExchangeDictionary (\w+) is empty")]
        public void GivenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var queue = (ExchangeDictionary)ScenarioContext.Current[exchangeDictionaryName];

            queue.IsEmpty.Should().BeTrue();
        }

        [Then(@"the scenario object ExchangeDictionary (\w+) is empty")]
        public void ThenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var queue = (ExchangeDictionary)ScenarioContext.Current[exchangeDictionaryName];

            queue.IsEmpty.Should().BeTrue();
        }
    }
}
