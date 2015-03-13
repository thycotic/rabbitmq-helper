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
    }
}