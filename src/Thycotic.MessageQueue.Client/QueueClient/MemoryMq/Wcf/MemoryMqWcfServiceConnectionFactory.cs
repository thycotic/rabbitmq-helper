using System;
using Thycotic.Logging;
using Thycotic.MemoryMq;
using Thycotic.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    internal class MemoryMqWcfServiceConnectionFactory
    {
        public string Uri
        {
            get { return _uri.AbsoluteUri; }
            set { _uri = new Uri(value); }
        }

        public int RequestedHeartbeat { get; set; }

        public object HostName
        {
            get { return _uri.Host; }
        }

        public bool UseSsl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        private Uri _uri;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqWcfServiceConnectionFactory));

        public IMemoryMqWcfServiceConnection CreateConnection()
        {
            try
            {

                var callback = new MemoryMqWcfServiceCallback();

                var channel = NetTcpChannelFactory.CreateDuplexChannel<IMemoryMqWcfService>(Uri, callback, UseSsl, Username, Password);
                
                return new MemoryMqWcfServiceConnection(channel, callback);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Connection failed to open because {0} ", ex.Message), ex);

                throw;
            }
        }
    }
}