using System.Collections.Concurrent;
using System.Linq;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Client dictionary
    /// </summary>
    public class ClientDictionary : IClientDictionary
    {
        private readonly ICallbackChannelProvider _callbackChannelProvider;

        private readonly ConcurrentDictionary<string, ClientList> _data =
            new ConcurrentDictionary<string, ClientList>();

        private readonly ILogWriter _log = Log.Get(typeof(ClientDictionary));

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDictionary"/> class.
        /// </summary>
        /// <param name="callbackChannelProvider">The callback channel provider.</param>
        public ClientDictionary(ICallbackChannelProvider callbackChannelProvider)
        {
            _callbackChannelProvider = callbackChannelProvider;
        }

        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void AddClient(string queueName)
        {
            var callback = _callbackChannelProvider.GetCallbackChannel(); 

            var client = new MemoryMqWcfServerClientProxy(callback.ToContextChannel(), callback);

            //have the consumer remove itself when it disconnects
            client.Channel.Closed += (sender, args) =>
            {
                _log.Debug("Detaching consumer");
                GetClientList(queueName).RemoveClient(client);
            };

            GetClientList(queueName).AddClient(client);
        }

        /// <summary>
        /// Tries the get client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="clientProxy">The client.</param>
        /// <returns></returns>
        public bool TryGetClient(string queueName, out MemoryMqWcfServerClientProxy clientProxy)
        {
            return GetClientList(queueName).TryGetClient(out clientProxy);
        }

        private ClientList GetClientList(string queueName)
        {
            lock (_data)
            {
                return _data.GetOrAdd(queueName, s => new ClientList());
            }
        }

        //TODO: Make own file
        /// <summary>
        /// 
        /// </summary>
        public class ClientList
        {
            private readonly ConcurrentDictionary<string, MemoryMqWcfServerClientProxy> _data = new ConcurrentDictionary<string, MemoryMqWcfServerClientProxy>();

            private int _robin;

            private readonly ILogWriter _log = Log.Get(typeof(ClientList));

            /// <summary>
            /// 
            /// </summary>
            /// <param name="clientProxy"></param>
            public void AddClient(MemoryMqWcfServerClientProxy clientProxy)
            {
                _log.Debug(string.Format("Adding consumer with session ID {0}", clientProxy.Channel.SessionId));

                _data.TryAdd(clientProxy.Channel.SessionId, clientProxy);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="clientProxy"></param>
            public void RemoveClient(MemoryMqWcfServerClientProxy clientProxy)
            {
                MemoryMqWcfServerClientProxy temp;
                _data.TryRemove(clientProxy.Channel.SessionId, out temp);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="clientProxy"></param>
            /// <returns></returns>
            public bool TryGetClient(out MemoryMqWcfServerClientProxy clientProxy)
            {
                var count = _data.Count;
                //simple round robin 
                if (count > 0)
                {
                    _robin = _robin % count;
                    clientProxy = _data.Values.Skip(_robin).First();
                    _robin++;
                    return true;
                }

                clientProxy = null;
                _robin = 0;
                return false;
            }
        }
    }
}