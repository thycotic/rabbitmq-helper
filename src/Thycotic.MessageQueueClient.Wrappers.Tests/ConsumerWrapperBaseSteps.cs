using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MessageQueue.Client.QueueClient;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    [Binding]
    public class ConsumerWrapperBaseSteps
    {
        [Given(@"there exists a ConsumerWrapperBaseDummy stored in the scenario as (\w+) with CommonConnection (\w+) and ExchangeNameProvider (\w+)")]
        public void GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenarioWithCommonConnectionAndExchangeNameProvider(string consumerWrapperName, string connectionName, string exchangeProviderName)
        {
            var connection = (ICommonConnection)ScenarioContext.Current[connectionName];
            var exchangeNameProvider = (IExchangeNameProvider)ScenarioContext.Current[exchangeProviderName];

            ScenarioContext.Current[consumerWrapperName] = new ConsumerWrapperBaseDummy(connection, exchangeNameProvider);
        }

        [When(@"the method StartConsuming on ConsumerWrapperBaseDummy (\w+) is called")]
        public void WhenTheMethodStartConsumingOnConsumerWrapperBaseDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = (ConsumerWrapperBaseDummy)ScenarioContext.Current[consumerWrapperName];

            consumerWrapper.StartConsuming();
        }

        [Then(@"the method ForceInitialize on CommonConnection substitute (\w+) is called")]
        public void ThenTheMethodForceInitializeOnCommonConnectionSubstituteIsCalled(string connectionName)
        {
            var connection = (ICommonConnection)ScenarioContext.Current[connectionName];

            connection.Received().ForceInitialize();  
        }
    }
}
