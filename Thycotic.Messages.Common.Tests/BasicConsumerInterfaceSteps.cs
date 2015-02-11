using System;
using TechTalk.SpecFlow;
using Thycotic.Utility.Specflow;

namespace Thycotic.Messages.Common.Tests
{
    [Binding]
    public class BasicConsumerInterfaceSteps
    {
        [Given(@"there exists a BasicConsumerDummy stored in the scenario as (\w+)")]
        public void GivenThereExistsABasicConsumerDummyStoredInTheScenario(string consumerName)
        {
            ScenarioContext.Current[consumerName] = new BasicConsumerDummy();
        }

        [When(@"the method Consumer on BasicConsumerDummy (\w+) is called with a null reference")]
        public void WhenTheMethodConsumerOnIBasicConsumerIsCalledWithANullReference(string consumerName)
        {
            ScenarioContext.Current.ExecuteThrowing<ApplicationException>(() =>
            {
                var consumer = (BasicConsumerDummy)ScenarioContext.Current[consumerName];
                consumer.Consume(null);
            });
        }
    }
}
