using System.Collections.Concurrent;
using System.Linq;
using Thycotic.Logging;
using Thycotic.Wcf;

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

            var channel = callback.ToContextChannel();

            //have the consumer remove itself when it disconnects
            channel.Closed += (sender, args) =>
            {
                _log.Debug("Detaching consumer");
                GetClientList(queueName).RemoveClient(callback);
            };

            GetClientList(queueName).AddClient(callback);
        }

        /// <summary>
        /// Tries the get client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="clientProxy">The client.</param>
        /// <returns></returns>
        public bool TryGetClient(string queueName, out IMemoryMqWcfServiceCallback clientProxy)
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
            private readonly ConcurrentDictionary<string, IMemoryMqWcfServiceCallback> _data = new ConcurrentDictionary<string, IMemoryMqWcfServiceCallback>();

            private int _robin;

            private readonly ILogWriter _log = Log.Get(typeof(ClientList));

            /// <summary>
            /// 
            /// </summary>
            /// <param name="callback"></param>
            public void AddClient(IMemoryMqWcfServiceCallback callback)
            {
                var channel = callback.ToContextChannel();

                _log.Debug(string.Format("Adding consumer with session ID {0}", channel.SessionId));
                
                _data.TryAdd(channel.SessionId, callback);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="callback"></param>
            public void RemoveClient(IMemoryMqWcfServiceCallback callback)
            {
                var channel = callback.ToContextChannel();
                
                IMemoryMqWcfServiceCallback temp;
                _data.TryRemove(channel.SessionId, out temp);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="clientProxy"></param>
            /// <returns></returns>
            public bool TryGetClient(out IMemoryMqWcfServiceCallback clientProxy)
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

            /// <summary>
            /// Removes all.
            /// </summary>
            public void RemoveAll()
            {
                _data.Values.ToList().ForEach(RemoveClient);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _data.Values.ToList().ForEach(cl => cl.RemoveAll());
        }
    }
}