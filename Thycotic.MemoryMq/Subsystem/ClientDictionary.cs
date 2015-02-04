﻿using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Client dictionary
    /// </summary>
    public class ClientDictionary
    {
        private readonly ConcurrentDictionary<string, ClientList> _data =
            new ConcurrentDictionary<string, ClientList>();

        private readonly ILogWriter _log = Log.Get(typeof(ClientDictionary));

        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void AddClient(string queueName)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMemoryMqServerCallback>();

            var client = new MemoryMqServerClientProxy(callback.GetChannel(), callback);

            //have the consumer remove itself when it disconnects
            client.Channel.Closed += (sender, args) =>
            {
                _log.Debug("Detaching consumer");
                GetConsumerList(queueName).RemoveConsumer(client);
            };

            GetConsumerList(queueName).AddConsumer(client);
        }

        /// <summary>
        /// Tries the get client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="clientProxy">The client.</param>
        /// <returns></returns>
        public bool TryGetClient(string queueName, out MemoryMqServerClientProxy clientProxy)
        {
            return GetConsumerList(queueName).TryGetConsumer(out clientProxy);
        }

        private ClientList GetConsumerList(string queueName)
        {
            return _data.GetOrAdd(queueName, s => new ClientList());
        }


        private class ClientList
        {
            private readonly ConcurrentDictionary<string, MemoryMqServerClientProxy> _data = new ConcurrentDictionary<string, MemoryMqServerClientProxy>();

            private int _robin;

            private readonly ILogWriter _log = Log.Get(typeof(ClientList));

            public void AddConsumer(MemoryMqServerClientProxy clientProxy)
            {
                _log.Debug(string.Format("Adding consumer with session ID {0}", clientProxy.Channel.SessionId));

                _data.TryAdd(clientProxy.Channel.SessionId, clientProxy);
            }

            public void RemoveConsumer(MemoryMqServerClientProxy clientProxy)
            {
                MemoryMqServerClientProxy temp;
                _data.TryRemove(clientProxy.Channel.SessionId, out temp);
            }

            public bool TryGetConsumer(out MemoryMqServerClientProxy clientProxy)
            {
                //simple round robin 
                if (_data.Any())
                {
                    _robin = _robin % _data.Count;
                    clientProxy = _data.Values.Skip(_robin).Single();
                    return true;
                }
                
                clientProxy = null;
                _robin = 0;
                return false;
            }
        }
    }
}