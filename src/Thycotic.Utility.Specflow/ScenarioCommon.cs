using System;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    [Binding]
    public class ScenarioCommon
    {
        public const string ScenarioException = "ScenarioException";

        [When(@"the string representation of scenario object (\w+) is stored in scenario object (\w+)")]
        public void WhenTheStringRepresentationOfScenarioObjectAndStoredInScenarioObject(string objectName, string resultsName)
        {
            this.GetScenarioContext().ExecuteThrowing<ApplicationException>(() =>
            {
                var obj = this.GetScenarioContext().Get<object>(objectName);
                this.GetScenarioContext().Set(resultsName, obj.ToString());
            });
        }

        [Then(@"value of scenario object (\w+) should be ""(.*)""")]
        public void ThenValueOfScenarioObjectRoutingSlipTestResultsShouldBe(string resultsName, string resultsString)
        {
            var str = this.GetScenarioContext().Get<string>(resultsName);

            str.Should().Be(resultsString);
        }

        [Then(@"the result stored in scenario as (\w+) is null")]
        public void ThenTheResultStoredInScenarioAsIsNull(string objectName)
        {
            //don't use the Get<T> method since it can't cast a null value to an object
            var obj = this.GetScenarioContext()[objectName];
            var isNull = obj == null;
            isNull.Should().Be(true);
        }

        [Then(@"the result stored in scenario as (\w+) is not null")]
        public void ThenTheResultStoredInScenarioAsIsNotNull(string objectName)
        {
            var obj = this.GetScenarioContext().Get<object>(objectName);
            obj.Should().NotBeNull();
        }
    }
}
