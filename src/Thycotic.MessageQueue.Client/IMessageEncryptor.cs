using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Interface for a message encryptor
    /// </summary>
    [ContractClass(typeof(MessageEncyptorContract))]
    public interface IMessageEncryptor
    {
        /// <summary>
        /// Encrypts the specified exchange name.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="body">To bytes.</param>
        /// <returns></returns>
        byte[] Encrypt(string exchangeName, byte[] body);

        /// <summary>
        /// Decrypts the specified exchange name.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        byte[] Decrypt(string exchangeName, byte[] body);
    }

    /// <summary>
    /// Contract for IMessageEncryptor
    /// </summary>
    [ContractClassFor(typeof (IMessageEncryptor))]
    public abstract class MessageEncyptorContract : IMessageEncryptor
    {
        /// <summary>
        /// Encrypts the specified exchange name.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="body">To bytes.</param>
        /// <returns></returns>
        public byte[] Encrypt(string exchangeName, byte[] body)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(exchangeName));
            Contract.Requires<ArgumentException>(body != null);

            return default(byte[]);
        }

        /// <summary>
        /// Decrypts the specified exchange name.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public byte[] Decrypt(string exchangeName, byte[] body)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(exchangeName));
            Contract.Requires<ArgumentException>(body != null);

            return default(byte[]);
        }
    }
}