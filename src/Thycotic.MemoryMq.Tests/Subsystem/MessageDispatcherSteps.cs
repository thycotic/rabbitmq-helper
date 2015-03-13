using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class MessageDispatcherSteps
    {

        [Given(@"there exists a substitute object for MessageDispatcher stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForMessageDispatcherStoredInTheScenario(string messageDispatcherName)
        {
            this.GetScenarioContext().SetSubstitute<IMessageDispatcher>(messageDispatcherName);
        }

    }
}
