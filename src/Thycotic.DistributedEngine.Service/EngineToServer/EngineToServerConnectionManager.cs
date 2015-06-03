using System;
using System.ServiceModel;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.Logic.Update;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Singleton that keeps track of the last valid Host/URL and uses it when generating connections.
    /// </summary>
    public class EngineToServerConnectionManager : IEngineToServerConnectionManager
    {       
        private string[] _connectionStrings;
        private readonly bool _useSsl;
        private static object _connectionLock = new object();

        private readonly ILogWriter _log = Log.Get(typeof(EngineToServerConnectionManager));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionStrings"></param>
        /// <param name="useSsl"></param>
        public EngineToServerConnectionManager(string connectionStrings, bool useSsl)
        {
            _connectionStrings = connectionStrings.Split(';');
            _useSsl = useSsl;
        }

        /// <summary>
        /// The currently active connection string for reaching the server.
        /// </summary>
        public string CurrentConnectionString
        {
            get
            {
                lock (_connectionLock)
                {
                    return _connectionStrings[0];
                }
            }
        }

        /// <summary>
        /// Open a channel to an accessible Server
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEngineToServerCommunicationWcfService OpenLiveChannel(IEngineToServerCommunicationCallback callback)
        {
            lock (_connectionLock)
            {
                int index = 0;
                while (index < _connectionStrings.Length)
                {
                    try
                    {
                        var connection = GetConnection(index);
                        var channel = connection.OpenChannel(callback);

                        channel.Ping();
                        string oldFirstElement = _connectionStrings[0];
                        _connectionStrings[0] = _connectionStrings[index];
                        _connectionStrings[index] = oldFirstElement;
                        return channel;
                    }
                    catch (Exception ex)
                    {
                        _log.Warn(string.Format("Problem reaching Url/Host {0} : {1}", _connectionStrings[index], ex));
                        index++;
                    }
                }
                var message = string.Format("No provided server could be reached using [{0}]", string.Join(";", _connectionStrings));
                throw new EndpointNotFoundException(message);
            }
        }

        /// <summary>
        /// Returns a live update web client.
        /// </summary>
        /// <returns></returns>
        public IUpdateWebClient OpenLiveUpdateWebClient()
        {            
            try
            {
                OpenLiveChannel(new EngineToServerCommunicationCallback());
                var connection = GetConnection();
                return connection.OpenUpdateWebClient();
            }
            catch (Exception ex)
            {
                _log.Warn(string.Format("Problem reaching Url/Host {0} : {1}", CurrentConnectionString, ex));
            }
            var message = string.Format("No provided server could be reached using [{0}]",
                CurrentConnectionString);
            throw new EndpointNotFoundException(message);
        }

        /// <summary>
        /// Used to get a Engine-Server connection for a particular index.
        /// Intended for use solely by this class.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual IEngineToServerConnection GetConnection(int index = 0)
        {
            return new EngineToServerConnection(_connectionStrings[index], _useSsl);
        }

        /// <summary>
        /// Dispose of managed / unmanaged resources
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}