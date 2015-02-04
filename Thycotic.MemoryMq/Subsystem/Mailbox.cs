using System.Collections.Concurrent;

namespace Thycotic.MemoryMq.Subsystem
{
    public class Mailbox
    {
        public string RoutingSlip { get; private set; }
        public QueueProxy Queue { get; private set; }

        public Mailbox(string routingSlip, ConcurrentQueue<byte[]> queue)
        {
            RoutingSlip = routingSlip;
            Queue = new QueueProxy(queue);
        }
    }
}