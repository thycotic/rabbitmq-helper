using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a client dictionary
    /// </summary>
    [ContractClass(typeof(ClientDictionaryContract))]
    public interface IClientDictionary : IDisposable
    {
        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        void AddClient(string queueName);

        /// <summary>
        /// Tries the get client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="clientProxy">The client.</param>
        /// <returns></returns>
        bool TryGetClient(string queueName, out IMemoryMqWcfServiceCallback clientProxy);
    }

    /// <summary>
    /// Contract for IClientDictionary
    /// </summary>
    [ContractClassFor(typeof(IClientDictionary))]
    public abstract class ClientDictionaryContract : IClientDictionary
    {
        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void AddClient(string queueName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
        }

        /// <summary>
        /// Tries the get client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="clientProxy">The client.</param>
        /// <returns></returns>
        public bool TryGetClient(string queueName, out IMemoryMqWcfServiceCallback clientProxy)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(queueName));
            Contract.ValueAtReturn(out clientProxy);
            Contract.Ensures(Contract.Result<bool>() && clientProxy != null);

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