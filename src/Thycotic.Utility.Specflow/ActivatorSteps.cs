using System;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    [Binding]
    public class ActivatorSteps
    {
        [Given(@"there exists an object of type ""(.+)"" stored in the scenario as (\w+)")]
        public void GivenThereExistsAnObjectOfTypeStoredInTheScenarioAs(string typeName, string testObjectNameInContext)
        {
            var type = Type.GetType(typeName);

            if (type == null)
            {
                throw new TypeLoadException(string.Format("Type {0} does not exist", typeName));
            }

            ScenarioContext.Current[testObjectNameInContext] = Activator.CreateInstance(type);
        }
    }
}