using System.Collections.Concurrent;

namespace Thycotic.MemoryMq.Subsystem
{
    public class QueueProxy
    {
        private readonly ConcurrentQueue<byte[]> _queue;

        public QueueProxy(ConcurrentQueue<byte[]> queue)
        {
            _queue = queue;
        }

        public bool TryDequeue(out byte[] body)
        {
            return _queue.TryDequeue(out body);
        }
    }
}