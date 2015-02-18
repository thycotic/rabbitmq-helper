using System;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq
{
    internal interface IMemoryMqServiceConnection
    {
        ICommonModel CreateModel();

        bool IsOpen { get; }

        EventHandler ConnectionShutdown { get; set; }

        void Close(int timeoutMilliseconds);
    }
}