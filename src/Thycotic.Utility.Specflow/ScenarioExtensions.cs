using System;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    public static class ScenarioExtensions
    {
        public static CustomScenarioContext GetScenarioContext(this object context)
        {
            return new CustomScenarioContext(ScenarioContext.Current);
        }

        public static void ExecuteThrowing<T>(this CustomScenarioContext context, Action action)
            where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T ex)
            {
                context.Set(ScenarioCommon.ScenarioException, ex.Message);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(
                    string.Format("Scenario was expecting an exception of type {0} but found one of type {1} ", typeof(T), ex.GetType()), ex);
            }
        }
    }
}