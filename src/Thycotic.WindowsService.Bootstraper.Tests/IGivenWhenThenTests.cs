using System;

namespace Thycotic.WindowsService.Bootstraper.Tests
{
    public interface IGivenWhenThenTests
    {
        void Given(Action promises);

        void When(Action actions);

        void Then(Action assertions);
    }
}