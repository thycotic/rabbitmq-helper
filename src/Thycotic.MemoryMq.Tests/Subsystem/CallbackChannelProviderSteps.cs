using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class CallbackChannelProviderSteps
    {
        [Given(@"there exists a substitute object for CallbackChannelProvider stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForCallbackChannelProviderStoredInTheScenario(string callbackChannelProviderName)
        {
            this.GetScenarioContext().SetSubstitute<ICallbackChannelProvider>(callbackChannelProviderName);
        }

    }
}
