using System;
using Thycotic.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    internal interface IMemoryMqWcfServiceConnection : IDuplexWcfConnection
    {
        ICommonModel CreateModel();
    }
}