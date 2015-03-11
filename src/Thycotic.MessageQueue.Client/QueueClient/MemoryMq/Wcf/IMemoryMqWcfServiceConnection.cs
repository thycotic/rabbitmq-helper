using System;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    internal interface IMemoryMqWcfServiceConnection : IDisposable
    {
        ICommonModel CreateModel();

        bool IsOpen { get; }

        EventHandler ConnectionShutdown { get; set; }

        void Close(int timeoutMilliseconds);
    }
}