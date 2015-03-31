using TechTalk.SpecFlow;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests
{
    [Binding]
    public class MemoryMqDeliveryEventArgsSteps
    {
        [Given(@"there exists a MemoryMqDeliveryEventArgs stored in the scenario as (\w+)")]
        public void GivenThereExistsAMemoryMqServerStoredInTheScenario(string eventArgsName)
        {
        
            this.GetScenarioContext().Set(eventArgsName, new MemoryMqDeliveryEventArgs());
        }
    }
}
