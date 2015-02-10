namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Mailbox
    /// </summary>
    public class Mailbox
    {
        /// <summary>
        /// Gets the routing slip.
        /// </summary>
        /// <value>
        /// The routing slip.
        /// </value>
        public RoutingSlip RoutingSlip { get; private set; }

        /// <summary>
        /// Gets the queue.
        /// </summary>
        /// <value>
        /// The queue.
        /// </value>
        public QueueProxy Queue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mailbox"/> class.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queue">The queue.</param>
        public Mailbox(RoutingSlip routingSlip, MessageQueue queue)
        {
            RoutingSlip = routingSlip;
            Queue = new QueueProxy(queue);
        }
    }
}