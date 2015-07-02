using System;

namespace Thycotic.DistributedEngine.Logic.Tests
{
    public interface IGivenWhenThenTests
    {
        void Given(Action promises);

        void When(Action actions);

        void Then(Action assertions);
    }
}