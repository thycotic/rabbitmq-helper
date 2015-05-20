using TechTalk.SpecFlow;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Tests.QueueClient
{
    [Binding]
    public class ExchangeNameProviderSteps
    {

        [Given(@"there exists a substitute object for IExchangeNameProvider stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForExchangeNameProviderStoredInTheScenario(string exchangeNameProvider)
        {
            this.GetScenarioContext().SetSubstitute<IExchangeNameProvider>(exchangeNameProvider);
        }
    }
}
