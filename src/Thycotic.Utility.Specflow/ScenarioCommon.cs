using FluentAssertions;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    [Binding]
    public class ScenarioCommon
    {
        public const string ScenarioException = "ScenarioException";

        [Then(@"the result stored in scenario as (\w+) is null")]
        public void ThenTheResultStoredInScenarioAsIsNull(string objectName)
        {
            //don't use the Get<T> method since it can't cast a null value to an object
            var obj = ScenarioContext.Current[objectName];
            var isNull = obj == null;
            isNull.Should().Be(true);
        }

        [Then(@"the result stored in scenario as (\w+) is not null")]
        public void ThenTheResultStoredInScenarioAsIsNotNull(string objectName)
        {
            var obj = ScenarioContext.Current.Get<object>(objectName);
            obj.Should().NotBeNull();
        }
    }
}
