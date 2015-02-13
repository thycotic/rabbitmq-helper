using Thycotic.TempAppCore;

namespace Thycotic.SecretServerEngine2.Security
{
    /// <summary>
    /// Tuple of Symmetric key and initialization vector.
    /// </summary>
    public class MessageEncryptionPair
    {
        /// <summary>
        /// The salt length
        /// </summary>
        public const int SaltLength = 8;

        /// <summary>
        /// Gets or sets the symmetric key.
        /// </summary>
        /// <value>
        /// The symmetric key.
        /// </value>
        public SymmetricKey SymmetricKey { get; set; }

        /// <summary>
        /// Gets or sets the initialization vector.
        /// </summary>
        /// <value>
        /// The initialization vector.
        /// </value>
        public InitializationVector InitializationVector { get; set; }
    }
}