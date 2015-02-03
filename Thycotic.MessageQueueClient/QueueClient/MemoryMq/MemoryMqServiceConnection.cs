using System;
using System.ServiceModel;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    internal class MemoryMqServiceConnection : IMemoryMqServiceConnection
    {
        private readonly IMemoryMqServiceClient _serviceClient;
        private readonly ICommunicationObject _communicationObject;
        
        public bool IsOpen
        {
            get { return _communicationObject.State == CommunicationState.Opened; }
        }

        public EventHandler ConnectionShutdown { get; set; }

        public MemoryMqServiceConnection(IMemoryMqServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
// ReSharper disable once SuspiciousTypeConversion.Global
            _communicationObject = (ICommunicationObject)serviceClient;

            Action<object, EventArgs> connectionShutdownHandler = (sender, args) =>
            {
                if (ConnectionShutdown != null)
                {
                    ConnectionShutdown(sender, args);
                }
            };

            _communicationObject.Faulted += (sender, args) => connectionShutdownHandler(sender, args);
            _communicationObject.Closed += (sender, args) => connectionShutdownHandler(sender, args);
            _communicationObject.Closing += (sender, args) => connectionShutdownHandler(sender, args);

        }

        public ICommonModel CreateModel()
        {
            return new MemoryMqModel(_serviceClient);
        }

        public void Close(int timeoutMilliseconds)
        {
            _communicationObject.Close(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        }
    }
}