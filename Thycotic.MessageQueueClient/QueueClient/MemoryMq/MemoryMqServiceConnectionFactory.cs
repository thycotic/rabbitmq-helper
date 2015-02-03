using System;
using System.ServiceModel;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    internal class MemoryMqServiceConnectionFactory
    {
        public string Uri { get; set; }
        public int RequestedHeartbeat { get; set; }
        public object HostName { get; set; }

        public IMemoryMqServiceConnection CreateConnection()
        {
            var clientBinding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
            clientBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            clientBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            var microwaveCallback = new MemoryMqServiceCallback();

            var channelFactory = new DuplexChannelFactory<IMemoryMqServiceClient>(microwaveCallback, clientBinding, Uri);
            //TODO: Do i need to worry about that since this is ephemeral? -dkk
            //channelFactory.Closed += new EventHandler(DuplexChannelFactory_Closed);
            //channelFactory.Closing += new EventHandler(DuplexChannelFactory_Closing);
            //channelFactory.Faulted += new EventHandler(DuplexChannelFactory_Faulted);
            
            var credentials = channelFactory.Credentials;

            if (credentials == null)
            {
                throw new ApplicationException("No credentials object");
            }

            credentials.UserName.UserName = Guid.NewGuid().ToString();
            credentials.UserName.Password = string.Empty;

            return new MemoryMqServiceConnection(channelFactory.CreateChannel());
        }
    }
}