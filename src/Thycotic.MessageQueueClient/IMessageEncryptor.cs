namespace Thycotic.MessageQueueClient
{
    /// <summary>
    /// Interface for a message encryptor
    /// </summary>
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
}