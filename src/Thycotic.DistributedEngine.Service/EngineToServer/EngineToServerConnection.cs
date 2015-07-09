using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.Logging;
using Thycotic.Wcf;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Wrapper for users of <see cref="IEngineToServerCommunicationWcfService"/> channels
    /// </summary>
    public class EngineToServerConnection : IEngineToServerConnection
    {
        private readonly string _connectionString;
        private readonly bool _useSsl;

        private readonly ILogWriter _log = Log.Get(typeof(EngineToServerConnection));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBus"/> class.
        /// </summary>
        /// <param name="connectionString">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public EngineToServerConnection(string connectionString, bool useSsl)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(connectionString));
            _connectionString = connectionString;
            _useSsl = useSsl;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        private string GetNetTcpCoreConnectionString()
        {
            return _connectionString;
        }

        private string GetHttpConnectionString(string route)
        {
            var baseUri = new Uri(_connectionString, UriKind.Absolute);

            //fixes things like /ihawu or /ss_qa
            var nestedRoute = string.Format("{0}/{1}", baseUri.AbsolutePath, route);

            var uri = new Uri(baseUri, new Uri(nestedRoute, UriKind.Relative));

            return uri.AbsoluteUri;
        }

        private string GetHttpCoreConnectionString()
        {
            const string route = IisEndPoints.RelativeIisHostedWcfServicePath;

            return GetHttpConnectionString(route);
        }

        private string GetHttpUpdateConnectionString()
        {
            var route = string.Format("{0}/{1}", IisEndPoints.RelativeIisHostedSupplementalControllerPath,
                IisEndPoints.SupplementalControllerActions.GetUpdate);

            return GetHttpConnectionString(route);
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Requested schema does not have a supported channel</exception>
        public IEngineToServerCommunicationWcfService OpenChannel(IEngineToServerCommunicationCallback callback)
        {
            var uri = new Uri(_connectionString);

            switch (uri.Scheme)
            {
                case "net.tcp":
                    _log.Info(string.Format("Using Net/TCP channel to {0}", _connectionString));
                    return NetTcpChannelFactory.CreateDuplexChannel<IDuplexEngineToServerCommunicationWcfService>(GetNetTcpCoreConnectionString(), callback, _useSsl);
                case "http":
                case "https":
                    _log.Info(string.Format("Using HTTP channel to {0}", _connectionString));
                    return HttpChannelFactory.CreateChannel<IUnidirectionalEngineToServerCommunicationWcfService>(GetHttpCoreConnectionString(), _useSsl);
                default:
                    throw new NotSupportedException("Requested schema does not have a supported channel");
            }
        }

        /// <summary>
        /// Opens the update web client.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Requested schema does not have a supported channel</exception>
        public IUpdateWebClient OpenUpdateWebClient()
        {
            var uri = new Uri(_connectionString);

            switch (uri.Scheme)
            {
                case "http":
                case "https":
                    _log.Info(string.Format("Using HTTP channel to {0}", _connectionString));
                    return new UpdateWebClient(GetHttpUpdateConnectionString());
                default:
                    throw new NotSupportedException("Requested schema does not have a supported channel");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //nothing to dispose
        }
    }
}