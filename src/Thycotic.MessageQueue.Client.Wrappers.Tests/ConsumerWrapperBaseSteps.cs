using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    [Binding]
    public class ConsumerWrapperBaseSteps
    {
        [Given(@"there exists a substitute object for ICommonConnection stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForCommonConnectionStoredInTheScenario(string commonConnectionName)
        {
            this.GetScenarioContext().SetSubstitute<ICommonConnection>(commonConnectionName);
        }

        [Given(@"there exists a substitute object for IExchangeNameProvider stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForExchangeNameProviderStoredInTheScenario(string exchangeNameProvider)
        {
            this.GetScenarioContext().SetSubstitute<IExchangeNameProvider>(exchangeNameProvider);
        }

        [Given(@"there exists a ConsumerWrapperBaseDummy stored in the scenario as (\w+) with CommonConnection (\w+) and ExchangeNameProvider (\w+)")]
        public void GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenarioWithCommonConnectionAndExchangeNameProvider(string consumerWrapperName, string connectionName, string exchangeProviderName)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
            var exchangeNameProvider = this.GetScenarioContext().Get<IExchangeNameProvider>(exchangeProviderName);

            this.GetScenarioContext().Set(consumerWrapperName, new ConsumerWrapperBaseDummy(connection, exchangeNameProvider));
        }

        [When(@"the method StartConsuming on ConsumerWrapperBaseDummy (\w+) is called")]
        public void WhenTheMethodStartConsumingOnConsumerWrapperBaseDummyIsCalled(string consumerWrapperName)
        {
            var consumerWrapper = this.GetScenarioContext().Get<ConsumerWrapperBaseDummy>(consumerWrapperName);

            consumerWrapper.StartConsuming();
        }

        [Then(@"the method ForceInitialize on ICommonConnection substitute (\w+) is called")]
        public void ThenTheMethodForceInitializeOnCommonConnectionSubstituteIsCalled(string connectionName)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);

            connection.Received().ForceInitialize();
        }
    }
}
