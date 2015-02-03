using System;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    internal class MemoryMqServiceConnectionFactory
    {
        public string Uri { get; set; }
        public int RequestedHeartbeat { get; set; }
        public object HostName { get; set; }

        public IMemoryMqServiceConnection CreateConnection()
        {
            throw new NotImplementedException();
        }
    }
}