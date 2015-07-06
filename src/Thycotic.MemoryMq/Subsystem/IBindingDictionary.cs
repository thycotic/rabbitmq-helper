using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a binding dictionary
    /// </summary>
    [ContractClass(typeof(BindingDictionaryContract))]
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

    /// <summary>
    /// Contract for IBindingDictionary
    /// </summary>
    [ContractClassFor(typeof(IBindingDictionary))]
    public abstract class BindingDictionaryContract : IBindingDictionary
    {
        /// <summary>
        /// Binds a queue to a routing slip
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queueName">Name of the queue.</param>
        public void AddBinding(RoutingSlip routingSlip, string queueName)
        {
            Contract.Requires<ArgumentNullException>(routingSlip != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
        }

        /// <summary>
        /// Tries to get a binding.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public bool TryGetBinding(RoutingSlip routingSlip, out string queueName)
        {
            Contract.Requires<ArgumentNullException>(routingSlip!=null);
            Contract.ValueAtReturn(out queueName);

            return default(bool);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}