using System;
using System.Linq.Expressions;
using System.ServiceModel;
using PostSharp.Aspects;
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
        private readonly string[] _connectionStrings;
        private int _currentIndex;
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
        /// Open a channel to an accessible Server
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEngineToServerCommunicationWcfService OpenLiveChannel(IEngineToServerCommunicationCallback callback)
        {
            lock (_connectionLock)
            {
                while (_currentIndex < _connectionStrings.Length)
                {
                    try
                    {
                        var connection = GetConnection();
                        var channel = connection.OpenChannel(callback);

                        if (channel is IUnidirectionalEngineToServerCommunicationWcfService)
                        {
                            channel.Ping();
                        }
                        return channel;
                    }
                    catch (Exception ex)
                    {
                        _log.Warn(string.Format("Problem reaching Url/Host {0} : {1}", _connectionStrings[_currentIndex], ex));
                        _currentIndex++;
                    }
                }
                _currentIndex = 0;
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
            lock (_connectionLock)
            {
                while (_currentIndex < _connectionStrings.Length)
                {
                    try
                    {
                        var connection = GetConnection();
                        var updateWebClient = connection.OpenUpdateWebClient();
                        updateWebClient.Ping();
                        return updateWebClient;
                    }
                    catch (Exception ex)
                    {
                        _log.Warn(string.Format("Problem reaching Url/Host {0} : {1}", _connectionStrings[_currentIndex], ex));
                        _currentIndex++;
                    }
                }
                _currentIndex = 0;
                var message = string.Format("No provided server could be reached using [{0}]", string.Join(";", _connectionStrings));
                throw new EndpointNotFoundException(message);
            }
        }

        private IEngineToServerConnection GetConnection()
        {
            return new EngineToServerConnection(_connectionStrings[_currentIndex], _useSsl);
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