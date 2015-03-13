using Thycotic.Utility.Specflow;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class MessageQueueSteps
    {
        [Given(@"there exists a MessageQueue stored in the scenario as (\w+)")]
        public void GivenThereExistsAnExchangeDictionaryStoredInTheScenario(string messageQueueName)
        {
            this.GetScenarioContext().Set(messageQueueName, new MessageQueue());
        }

        [Given(@"there exists a substitute object for MessageQueue stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForExchangeDictionaryStoredInTheScenario(string messageQueueName)
        {
            this.GetScenarioContext().SetSubstitute<IMessageQueue>(messageQueueName);
        }

    }
}
