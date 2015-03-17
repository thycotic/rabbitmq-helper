using System;
using System.Collections.Generic;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for an exchange dictionary
    /// </summary>
    public interface IExchangeDictionary : IDisposable
    {
        /// <summary>
        /// Gets the mailboxes in the exchange
        /// </summary>
        /// <value>
        /// The mailboxes.
        /// </value>
        IList<Mailbox> Mailboxes { get; }

        /// <summary>
        /// Gets a value indicating whether this exchange is empty. This empty mailboxes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        bool IsEmpty { get; }

        /// <summary>
        /// Publishes the body to the specified routing slip.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="body">The body.</param>
        void Publish(RoutingSlip routingSlip, MemoryMqDeliveryEventArgs body);

        /// <summary>
        /// Acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="routingSlip">The routing slip.</param>
        /// <exception cref="System.ApplicationException">Delivery tag was not found</exception>
        void Acknowledge(ulong deliveryTag, RoutingSlip routingSlip);

        /// <summary>
        /// Negatively acknowledges.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="routingSlip">The routing slip.</param>
        /// <exception cref="System.ApplicationException">Delivery tag was not found</exception>
        void NegativelyAcknowledge(ulong deliveryTag, RoutingSlip routingSlip);

        /// <summary>
        /// Restores the persisted messages from disk to memory.
        /// </summary>
        void RestorePersistedMessages();

        /// <summary>
        /// Persists the messages from memory to disk.
        /// </summary>
        void PersistMessages();
    }
}