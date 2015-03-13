using NSubstitute;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    public static class SubstituteExtensions
    {
        public static T GetSubstitute<T>(this ScenarioContext context)
            where T : class
        {
            return Substitute.For<T>();
        }

        public static T1 GetSubstitute<T1, T2>(this ScenarioContext context)
            where T1 : class
            where T2 : class
        {
            return Substitute.For<T1, T2>();
        }

        public static T1 GetSubstitute<T1, T2, T3>(this ScenarioContext context)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            return Substitute.For<T1, T2, T3>();
        }
    }
}