using System;
using System.Security.Cryptography.X509Certificates;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.Security;
using Thycotic.Logging;
using Thycotic.Utility.Serialization;
using Thycotic.Wcf;
using PublicKey = Thycotic.Encryption.PublicKey;

namespace Thycotic.DistributedEngine
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
        /// <param name="publicKey"></param>
        /// <param name="objectSerializer"></param>
        /// <param name="engineToServerEncryptor"></param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Requested schema does not have a supported channel</exception>
        public IEngineToServerChannel OpenChannel(PublicKey publicKey, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor)
        {
            var uri = new Uri(_connectionString);

            IEngineToServerCommunicationWcfService2 connection;

            switch (uri.Scheme)
            {
                case "net.tcp":
                    _log.Info(string.Format("Using Net/TCP channel to {0}", _connectionString));
                    connection= null;// NetTcpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(_connectionString, _useSsl);
                    break;
                case "http":
                case "https":
                    _log.Info(string.Format("Using HTTP channel to {0}", _connectionString));
                    connection = null;// HttpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(_connectionString, _useSsl);
                    break;
                default:
                    throw new NotSupportedException("Requested schema does not have a supported channel");
            }

            return new EngineToServerChannel(publicKey, connection, objectSerializer, engineToServerEncryptor);


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