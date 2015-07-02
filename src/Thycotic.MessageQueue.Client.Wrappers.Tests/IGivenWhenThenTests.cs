using System;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    public interface IGivenWhenThenTests
    {
        void Given(Action promises);

        void When(Action actions);

        void Then(Action assertions);
    }
}