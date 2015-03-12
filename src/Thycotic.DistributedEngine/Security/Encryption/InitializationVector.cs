namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// InitializationVector
    /// </summary>
    public class InitializationVector : KeyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationVector"/> class.
        /// </summary>
        /// <param name="initializationVector">The initialization vector.</param>
        public InitializationVector(byte[] initializationVector)
            : base(initializationVector)
        {
        }
    }
}
