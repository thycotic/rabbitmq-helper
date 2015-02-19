namespace Thycotic.DistributedEngine.Logic
{
    /// <summary>
    /// Interface for a remote configuration provider
    /// </summary>
    public interface IRestCommunicationProvider
    {
        /// <summary>
        /// Posts the specified request to the specified path.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResult Post<TResult>(string path, object request);
    }
}