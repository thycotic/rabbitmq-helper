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
        [Given(@"there exists a substitute object for IBasicConsumable stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIBasicConsumableStoredInTheScenario(string consumableName)
        {
            this.GetScenarioContext().SetSubstitute<IBasicConsumable>(consumableName);
        }

        [Given(@"there exists a substitute object for IBlockingConsumable stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIBlockingConsumableStoredInTheScenario(string consumableName)
        {
            this.GetScenarioContext().SetSubstitute<IBlockingConsumable>(consumableName);
        }
    }
}
