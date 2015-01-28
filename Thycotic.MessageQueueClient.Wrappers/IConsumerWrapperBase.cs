using System;

namespace Thycotic.MessageQueueClient.Wrappers
{
    public interface IConsumerWrapperBase : IDisposable
    {
        void StartConsuming();
    }
}