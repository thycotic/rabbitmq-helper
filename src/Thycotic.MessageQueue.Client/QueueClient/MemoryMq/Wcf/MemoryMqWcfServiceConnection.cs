using System;
using System.ServiceModel;
using Thycotic.Logging;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    internal class MemoryMqWcfServiceConnection : IMemoryMqWcfServiceConnection
    {
        private readonly IMemoryMqWcfServer _server;

        private readonly ICommunicationObject _communicationObject;
        private readonly MemoryMqWcfServiceCallback _callback;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqWcfServiceConnection));

        public bool IsOpen
        {
            get { return _communicationObject.State == CommunicationState.Opened; }
        }

        public EventHandler ConnectionShutdown { get; set; }

        public MemoryMqWcfServiceConnection(IMemoryMqWcfServer server, MemoryMqWcfServiceCallback callback)
        {
            _server = server;
            _callback = callback;
            _communicationObject = server.ToCommunicationObject();

            Action<object, EventArgs> connectionShutdownHandler = (sender, args) =>
            {
                if (ConnectionShutdown != null)
                {
                    ConnectionShutdown(sender, args);
                }
            };

            _communicationObject.Faulted += (sender, args) => connectionShutdownHandler(sender, args);
            _communicationObject.Closed += (sender, args) => connectionShutdownHandler(sender, args);
        }

        public ICommonModel CreateModel()
        {
            return new MemoryMqModel(_server, _callback);
        }

        public void Close(int timeoutMilliseconds)
        {
            try
            {
                _communicationObject.Close(TimeSpan.FromMilliseconds(timeoutMilliseconds));
            }
            catch (Exception ex)
            {
                _log.Error("Failed to close connection", ex);
            }
        }
    }
}