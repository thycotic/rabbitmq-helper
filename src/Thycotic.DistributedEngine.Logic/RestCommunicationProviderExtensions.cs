using System;

namespace Thycotic.DistributedEngine.Logic
{
    /// <summary>
    /// Rest communication provider extensions
    /// </summary>
    public static class RestCommunicationProviderExtensions
    {

        /// <summary>
        /// Gets the endpoint URI.
        /// </summary>
        /// <param name="restCommunicationProvider">The rest communication provider.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static Uri GetEndpointUri(this IRestCommunicationProvider restCommunicationProvider, string prefix, string action)
        {
            return new Uri(string.Format("{0}/{1}", prefix, action), UriKind.Relative);
        }
    }
}