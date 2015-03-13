using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    [Binding]
    public class ConsumerWrapperBaseSteps
    {
        [Given(@"there exists a ConsumerWrapperBaseDummy stored in the scenario as (\w+) with CommonConnection (\w+) and ExchangeNameProvider (\w+)")]
        public void GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenarioWithCommonConnectionAndExchangeNameProvider(string consumerWrapperName, string connectionName, string exchangeProviderName)
        {
            var connection = ScenarioContext.Current.Get<ICommonConnection>(connectionName);
            var exchangeNameProvider = ScenarioContext.Current.Get<IExchangeNameProvider>(exchangeProviderName);

            ScenarioContext.Current.Set(consumerWrapperName, new ConsumerWrapperBaseDummy(connection, exchangeNameProvider));
        }

        [When(@"the method StartConsuming on ConsumerWrapperBaseDummy (\w+) is called")]
        public void WhenTheMethodStartConsumingOnConsumerWrapperBaseDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = ScenarioContext.Current.Get<ConsumerWrapperBaseDummy>(consumerWrapperName);

            consumerWrapper.StartConsuming();
        }

        [Then(@"the method ForceInitialize on CommonConnection substitute (\w+) is called")]
        public void ThenTheMethodForceInitializeOnCommonConnectionSubstituteIsCalled(string connectionName)
        {
            var connection = ScenarioContext.Current.Get<ICommonConnection>(connectionName);

            connection.Received().ForceInitialize();  
        }
    }
}
