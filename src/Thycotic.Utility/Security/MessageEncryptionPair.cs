namespace Thycotic.Utility.Security
{

    /// <summary>
    /// Message encryption
    /// </summary>
    public static class MessageEncryption
    {
        /// <summary>
        /// The salt length
        /// </summary>
        public const int SaltLength = 8;
    }

    /// <summary>
    /// Tuple of Symmetric key and initialization vector.
    /// </summary>
    public class MessageEncryptionPair<TSymmetricKey, TInitializationVector>
    {


        /// <summary>
        /// Gets or sets the symmetric key.
        /// </summary>
        /// <value>
        /// The symmetric key.
        /// </value>
        public TSymmetricKey SymmetricKey { get; set; }

        /// <summary>
        /// Gets or sets the initialization vector.
        /// </summary>
        /// <value>
        /// The initialization vector.
        /// </value>
        public TInitializationVector InitializationVector { get; set; }
    }
}