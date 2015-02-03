using System;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    internal class MemoryMqServiceConnection : IMemoryMqServiceConnection
    {
        private readonly IMemoryMqServiceClient _createChannel;

        public MemoryMqServiceConnection(IMemoryMqServiceClient createChannel)
        {
            _createChannel = createChannel;
        }

        public ICommonModel CreateModel()
        {
            return new MemoryMqModel(_createChannel);
        }

        public bool IsOpen { get; set; }
        public EventHandler ConnectionShutdown { get; set; }
        public void Close(int timeoutMilliseconds)
        {
            throw new NotImplementedException();
        }
    }
}