namespace Thycotic.SecretServerEngine2.MemoryMq
{
    /// <summary>
    /// Interface for engine client verifier
    /// </summary>
    public interface IEngineClientVerifier
    {
        /// <summary>
        /// Validates the specified agentkey.
        /// </summary>
        /// <param name="agentkey">The agentkey.</param>
        void Validate(string agentkey);
    }
}