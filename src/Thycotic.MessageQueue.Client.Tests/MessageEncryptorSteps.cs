using TechTalk.SpecFlow;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Tests
{
    [Binding]
    public class MessageEncryptorSteps
    {
        [Given(@"there exists a substitute object for IMessageEncryptor stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIMessageEncryptorStoredInTheScenario(string messageEncryptorName)
        {
            this.GetScenarioContext().SetSubstitute<IMessageEncryptor>(messageEncryptorName);
        }
    }
}
