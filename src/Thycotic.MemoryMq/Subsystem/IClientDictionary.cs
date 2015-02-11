namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a client dictionary
    /// </summary>
    public interface IClientDictionary
    {
        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        void AddClient(string queueName);

        /// <summary>
        /// Tries the get client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="clientProxy">The client.</param>
        /// <returns></returns>
        bool TryGetClient(string queueName, out MemoryMqServerClientProxy clientProxy);
    }
}