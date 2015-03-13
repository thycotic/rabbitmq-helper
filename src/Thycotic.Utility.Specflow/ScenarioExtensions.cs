using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    public static class ScenarioExtensions
    {
        public static void Set<T>(this ScenarioContext context, string key, T data)
        {
            context.Set(data, key);
        }

        public static void ExecuteThrowing<T>(this ScenarioContext context, Action action)
            where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T ex)
            {
                context.Add(ScenarioCommon.ScenarioException, ex.Message);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(
                    string.Format("Scenario was expecting an exception of type {0} but found one of type {1} ", typeof(T), ex.GetType()), ex);
            }
        }
    }
}