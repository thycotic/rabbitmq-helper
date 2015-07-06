using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for an exchange dictionary
    /// </summary>
    [ContractClass(typeof(ExchangeDictionaryContract))]
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
        /// <param name="requeue"></param>
        /// <exception cref="System.ApplicationException">Delivery tag was not found</exception>
        void NegativelyAcknowledge(ulong deliveryTag, RoutingSlip routingSlip, bool requeue);

        /// <summary>
        /// Restores the persisted messages from disk to memory.
        /// </summary>
        void RestorePersistedMessages();

        /// <summary>
        /// Persists the messages from memory to disk.
        /// </summary>
        void PersistMessages();
    }

    /// <summary>
    /// Contract for Exchange Dictionary
    /// </summary>
    [ContractClassFor(typeof(IExchangeDictionary))]
    public abstract class ExchangeDictionaryContract : IExchangeDictionary
    {
        /// <summary>
        /// Gets the mailboxes in the exchange
        /// </summary>
        /// <value>
        /// The mailboxes.
        /// </value>
        public IList<Mailbox> Mailboxes { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this exchange is empty. This empty mailboxes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// Publishes the body to the specified routing slip.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="body">The body.</param>
        public void Publish(RoutingSlip routingSlip, MemoryMqDeliveryEventArgs body)
        {
            Contract.Requires<ArgumentNullException>(routingSlip != null);
            Contract.Requires<ArgumentNullException>(body != null);
        }

        /// <summary>
        /// Acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="routingSlip">The routing slip.</param>
        /// <exception cref="System.ApplicationException">Delivery tag was not found</exception>
        public void Acknowledge(ulong deliveryTag, RoutingSlip routingSlip)
        {
            Contract.Requires<ArgumentNullException>(routingSlip != null);
        }

        /// <summary>
        /// Negatively acknowledges.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="requeue"></param>
        /// <exception cref="System.ApplicationException">Delivery tag was not found</exception>
        public void NegativelyAcknowledge(ulong deliveryTag, RoutingSlip routingSlip, bool requeue)
        {
            Contract.Requires<ArgumentNullException>(routingSlip != null);
        }

        /// <summary>
        /// Restores the persisted messages from disk to memory.
        /// </summary>
        public void RestorePersistedMessages()
        {
        }

        /// <summary>
        /// Persists the messages from memory to disk.
        /// </summary>
        public void PersistMessages()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}