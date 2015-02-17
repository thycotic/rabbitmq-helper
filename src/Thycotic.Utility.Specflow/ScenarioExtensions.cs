using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    public static class ScenarioExtensions
    {
        public static Type GetLoadedType(this ScenarioContext context, string typeName)
        {
            //try getting the type from currently loaded assemblies
            var type = Type.GetType(typeName);

            if (type != null)
            {
                return type;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var aggregate = new AggregateException(assemblies.OrderBy(a => a.GetName().Name).Select(a => new TypeLoadException(a.FullName)));

            throw new TypeLoadException(string.Format("Type {0} does not exist", typeName), aggregate);
        }

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
}