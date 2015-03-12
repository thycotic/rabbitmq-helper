namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// Symmetric Key
    /// </summary>
    public class SymmetricKey : KeyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricKey"/> class.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        public SymmetricKey(byte[] symmetricKey)
            : base(symmetricKey)
        {
        }
    }
}
