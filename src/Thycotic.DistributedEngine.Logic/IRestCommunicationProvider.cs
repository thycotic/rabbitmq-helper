using System;

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
        /// <param name="uri">The URI.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResult Post<TResult>(Uri uri, object request);
    }
}