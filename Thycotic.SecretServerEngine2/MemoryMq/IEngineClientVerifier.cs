namespace Thycotic.SecretServerEngine2.MemoryMq
{
    /// <summary>
    /// Interface for engine client verifier
    /// </summary>
    public interface IEngineClientVerifier
    {
        /// <summary>
        /// Validates the specified clientKey.
        /// </summary>
        /// <param name="clientKey">The clientKey.</param>
        void Validate(string clientKey);
    }
}