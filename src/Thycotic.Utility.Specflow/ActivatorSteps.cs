using System;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    [Binding]
    public class ActivatorSteps
    {
        [Given(@"there is a null value stored in the scenario as (\w+)")]
        public void GivenThereIsANullStoredInTheScenarioAs(string testObjectNameInContext)
        {
            this.GetScenarioContext().SetNull(testObjectNameInContext);
        }

        [Given(@"there exists a Boolean object stored in the scenario as (\w+)")]
        public void GivenThereExistsABooleanObjectStoredInTheScenario(string booleanName)
        {
            this.GetScenarioContext().Set(booleanName, new Boolean());
        }

    }
}