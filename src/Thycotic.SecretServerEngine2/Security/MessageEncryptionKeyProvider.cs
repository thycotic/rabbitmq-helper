using Thycotic.TempAppCore;

namespace Thycotic.SecretServerEngine2.Security
{
    /// <summary>
    /// Message encryption provider
    /// </summary>
    public class MessageEncryptionKeyProvider : IMessageEncryptionKeyProvider
    {
        /// <summary>
        /// Tries the get key.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="symmetricKey">The skey.</param>
        /// <param name="initializationVector">The iv.</param>
        /// <returns></returns>
        public bool TryGetKey(string exchangeName, out SymmetricKey symmetricKey, out InitializationVector initializationVector)
        {
            symmetricKey = null;
            initializationVector = null;
            return false;
        }
    }
}