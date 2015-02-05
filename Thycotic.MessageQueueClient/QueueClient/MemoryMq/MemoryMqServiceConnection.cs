using System;
using System.ServiceModel;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    internal class MemoryMqServiceConnection : IMemoryMqServiceConnection
    {
        private readonly IMemoryMqServer _server;

        private readonly ICommunicationObject _communicationObject;
        private readonly MemoryMqServiceCallback _callback;

        public bool IsOpen
        {
            get { return _communicationObject.State == CommunicationState.Opened; }
        }

        public EventHandler ConnectionShutdown { get; set; }

        public MemoryMqServiceConnection(IMemoryMqServer server, MemoryMqServiceCallback callback)
        {
            _server = server;
            _callback = callback;
            _communicationObject = server.GetCommunicationObject();

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
            _communicationObject.Close(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        }
    }
}