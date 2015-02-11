using System;
using FluentAssertions;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class RoutingSlipSteps
    {
        [Given(@"there exists a RoutingSlip stored in the scenario as (\w+) with exchange (\w+) and routing key (\w+)")]
        public void GivenThereExistsARoutingSlipStoredInTheScenarioAsRoutingSlipTestWithExchangeTestChangeAndRoutingKeyTestRoutingKey(string routingSlipName, string exchangeName, string routingKey)
        {
            ScenarioContext.Current[routingSlipName] = new RoutingSlip(exchangeName, routingKey);
        }

        [Given(@"there exists a RoutingSlip stored in the scenario as (\w+) with no exchange and routing key (\w+)")]
        public void GivenThereExistsARoutingSlipStoredInTheScenarioAsRoutingSlipTestWithNoExchangeAndRoutingKeyTestRoutingKey(string routingSlipName, string routingKey)
        {
            ScenarioContext.Current[routingSlipName] = new RoutingSlip(string.Empty, routingKey);
        }

        [Given(@"there exists a RoutingSlip stored in the scenario as (\w+) with exchange (\w+) and no routing key")]
        public void GivenThereExistsARoutingSlipStoredInTheScenarioAsRoutingSlipTestWithExchangeTestChangeAndNoRoutingKey(string routingSlipName, string exchangeName)
        {
            ScenarioContext.Current[routingSlipName] = new RoutingSlip(exchangeName, string.Empty);
        }

        [When(@"the string representation of scenario object (\w+) and stored in scenario object (\w+)")]
        public void WhenTheStringRepresentationOfScenarioObjectRoutingSlipTestAndStoredInScenarioObjectRoutingSlipTestResults(string routingSlipName, string routingSlipResults)
        {
            ScenarioContext.Current.ExecuteThrowing<ApplicationException>(() =>
            {
                var routingSlip = (RoutingSlip) ScenarioContext.Current[routingSlipName];
                ScenarioContext.Current[routingSlipResults] = routingSlip.ToString();
            });
        }

        [Then(@"value of scenario object (\w+) should be ""(.*)""")]
        public void ThenValueOfScenarioObjectRoutingSlipTestResultsShouldBe(string routingSlipResults, string resultsString)
        {
            var str = (string)ScenarioContext.Current[routingSlipResults];

            str.Should().Be(resultsString);
        }
    }
}
