using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class BindingDictionarySteps
    {
        [Given(@"there exists a substitute object for BindingDictionary stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForBindingDictionaryStoredInTheScenario(string bindingDictionaryName)
        {
            this.GetScenarioContext().SetSubstitute<IBindingDictionary>(bindingDictionaryName);
        }
    }
}
