using System;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a binding dictionary
    /// </summary>
    public interface IBindingDictionary : IDisposable
    {
        /// <summary>
        /// Binds a queue to a routing slip
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queueName">Name of the queue.</param>
        void AddBinding(RoutingSlip routingSlip, string queueName);

        /// <summary>
        /// Tries to get a binding.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        bool TryGetBinding(RoutingSlip routingSlip, out string queueName);
    }
}