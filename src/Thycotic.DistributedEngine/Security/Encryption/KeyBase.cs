namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// KeyBase
    /// </summary>
    public abstract class KeyBase : IKey
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public byte[] Value
        {
            get;
            internal set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBase"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        protected KeyBase(byte[] key)
        {
            Value = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBase"/> class.
        /// </summary>
        protected KeyBase()
        {
        }
    }
}
