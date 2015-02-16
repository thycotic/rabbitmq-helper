using Thycotic.AppCore.Cryptography;
using Thycotic.ihawu.Business.DoubleLock.Cryptography.KeyTypes;

namespace Thycotic.SecretServerEngine2.Security
{
    /// <summary>
    /// Interface for a message encryption provider
    /// </summary>
    public interface IMessageEncryptionKeyProvider
    {
        /// <summary>
        /// Tries the get key.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="symmetricKey">The skey.</param>
        /// <param name="initializationVector">The iv.</param>
        /// <returns></returns>
        bool TryGetKey(string exchangeName, out SymmetricKey symmetricKey, out InitializationVector initializationVector);
    }
}