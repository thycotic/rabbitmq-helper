using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Thycotic.MemoryMq.Tests
{
    public static class ScenarioCommon
    {
        public const string ScenarioException = "ScenarioException";
    }

    [Binding]
    public class ScenarioExceptions
    {

        [Then(@"there should have been a exception thrown with message ""(.*)""")]
        public void ThenThereShouldHaveBeenAExceptionThrownWithMessage(string exceptionMessage)
        {
            var message = (string)ScenarioContext.Current[ScenarioCommon.ScenarioException];
            message.Should().Be(exceptionMessage);
        }
    }
}
