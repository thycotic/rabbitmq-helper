using System;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    public static class ScenarioCommon
    {
        public const string ScenarioException = "ScenarioException";
    }

    public static class ScenarioExtensions
    {
        public static void ExecuteThrowing<T>(this ScenarioContext context, Action action)
            where T: Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T ex)
            {
                context[ScenarioCommon.ScenarioException] = ex.Message;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(
                    string.Format("Scenario was expecting an exception of type {0} but found one of type {1} ", typeof (T), ex.GetType()), ex);
            }
        }
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
