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
            var obj = ScenarioContext.Current.Get<object>(objectName);
            obj.Should().BeNull();
        }

        [Then(@"the result stored in scenario as (\w+) is not null")]
        public void ThenTheResultStoredInScenarioAsIsNotNull(string objectName)
        {
            var obj = ScenarioContext.Current.Get<object>(objectName);
            obj.Should().NotBeNull();
        }
    }
}
