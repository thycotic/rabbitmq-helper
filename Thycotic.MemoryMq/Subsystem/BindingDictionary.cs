using System.Collections.Concurrent;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Bindings dictionary 
    /// </summary>
    public class BindingDictionary
    {
        private readonly ConcurrentDictionary<RoutingSlip, string> _data = new ConcurrentDictionary<RoutingSlip, string>();

        private readonly ILogWriter _log = Log.Get(typeof(BindingDictionary));

        /// <summary>
        /// Binds a queue to a routing slip
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queueName">Name of the queue.</param>
        public void AddBinding(RoutingSlip routingSlip, string queueName)
        {
            _log.Debug(string.Format("Adding binding for routing slip {0} to {1} queue", routingSlip, queueName));

            _data.TryAdd(routingSlip, queueName);
        }

        /// <summary>
        /// Tries to get a binding.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public bool TryGetBinding(RoutingSlip routingSlip, out string queueName)
        {
            return _data.TryGetValue(routingSlip, out queueName);
        }
    }
}