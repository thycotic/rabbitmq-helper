using System;
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
        /// Initializes a new instance of the <see cref="EngineConfigurationBus"/> class.
        /// </summary>
        /// <param name="connectionString">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public EngineToServerConnection(string connectionString, bool useSsl)
        {
            _connectionString = connectionString;
            _useSsl = useSsl;
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Requested schema does not have a supported channel</exception>
        public IEngineToServerCommunicationWcfService OpenChannel()
        {
            var uri = new Uri(_connectionString);

            switch (uri.Scheme)
            {
                case "net.tcp":
                    _log.Info(string.Format("Using Net/TCP channel to {0}", _connectionString));
                    return NetTcpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(_connectionString, _useSsl);
                case "http":
                case "https":
                    _log.Info(string.Format("Using HTTP channel to {0}", _connectionString));
                    return HttpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(_connectionString, _useSsl);
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