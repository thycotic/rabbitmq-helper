using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;
using Thycotic.Wcf;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class CallbackChannelProviderSteps
    {
        [Given(@"there exists a substitute object for ICallbackChannelProvider stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForCallbackChannelProviderStoredInTheScenario(string callbackChannelProviderName)
        {
            this.GetScenarioContext().SetSubstitute<ICallbackChannelProvider>(callbackChannelProviderName);
        }

    }
}
