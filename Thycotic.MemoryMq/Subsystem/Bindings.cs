using System.Collections.Concurrent;
using System.Dynamic;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Bindings which bind queues, to topics
    /// </summary>
    public class Bindings
    {
        private readonly ConcurrentDictionary<RoutingSlip, string> _data = new ConcurrentDictionary<RoutingSlip, string>();

        private readonly ILogWriter _log = Log.Get(typeof(Bindings));

        /// <summary>
        /// Binds a queue to a routing slip
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queueName">Name of the queue.</param>
        public void AddBinding(RoutingSlip routingSlip, string queueName)
        {
            _data.TryAdd(routingSlip, queueName);
        }

        public bool TryGetBinding(RoutingSlip routingSlip, out string queueName)
        {
            return _data.TryGetValue(routingSlip, out queueName);
        }
    }
}