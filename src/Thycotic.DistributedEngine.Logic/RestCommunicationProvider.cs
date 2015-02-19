using ServiceStack;
using Thycotic.Logging;

namespace Thycotic.SecretServerEngine.Logic
{
    /// <summary>
    /// Remote configuration provider
    /// </summary>
    public class RestCommunicationProvider : IRestCommunicationProvider
    {
        private readonly JsonServiceClient _serviceClient;
        private readonly ILogWriter _log = Log.Get(typeof(RestCommunicationProvider));

        /// <summary>
        /// Messages the encryptor.
        /// </summary>
        /// <param name="url">The URL.</param>
        public RestCommunicationProvider(string url)
        {
            _serviceClient = new JsonServiceClient(url);
        }

        /// <summary>
        /// Posts the specified request to the specified path.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResult Post<TResult>(string path, object request)
        {
            _log.Debug(string.Format("Posting to {0}", path));

            return _serviceClient.Send<TResult>("POST", path, request);
        }
    }
}