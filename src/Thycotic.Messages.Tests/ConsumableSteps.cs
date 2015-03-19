using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Thycotic.Messages.Common;
using Thycotic.Utility.Specflow;

namespace Thycotic.Messages.Tests
{
    [Binding]
    public class ConsumableSteps
    {
        [Given(@"there exists a substitute object for IConsumable stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIConsumableStoredInTheScenario(string consumableName)
        {
            this.GetScenarioContext().SetSubstitute<IConsumable>(consumableName);
        }
    }
}
