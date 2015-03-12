namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// EncryptedPrivateKey
    /// </summary>
    public class EncryptedPrivateKey : KeyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedPrivateKey"/> class.
        /// </summary>
        /// <param name="encryptedPrivateKey">The encrypted private key.</param>
        public EncryptedPrivateKey(byte[] encryptedPrivateKey)
            : base(encryptedPrivateKey)
        {
        }
    }
}
