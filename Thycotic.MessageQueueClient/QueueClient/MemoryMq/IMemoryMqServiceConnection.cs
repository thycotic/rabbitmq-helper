using System;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    internal interface IMemoryMqServiceConnection
    {
        ICommonModel CreateModel();

        bool IsOpen { get; set; }

        EventHandler ConnectionShutdown { get; set; }

        void Close(int timeoutMilliseconds);
    }
}