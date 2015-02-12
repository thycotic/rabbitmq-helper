using System;
using NSubstitute;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    [Binding]
    public class SubstituteSteps
    {
        private static object GetSubstitute(string typeName)
        {
            var type = ScenarioContext.Current.GetLoadedType(typeName);

            //TODO: Check to make sure that the type has tests requested for it -dkk

            return Substitute.For(new[] {type}, null);
        }

        [Given(@"there exists a substitute object of type ""(.+)"" stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteStoredInTheScenario(string typeName, string testObjectNameInContext)
        {
            ScenarioContext.Current[testObjectNameInContext] = GetSubstitute(typeName);
        }

    }
}