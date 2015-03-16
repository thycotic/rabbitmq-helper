using System.ServiceModel;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Channel factory
    /// </summary>
    public static class NetTcpChannelFactory
    {

        /// <summary>
        /// Creates a duplex channel.
        /// </summary>
        /// <typeparam name="TServer">The type of the server.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static TServer CreateDuplexChannel<TServer>(string uri, bool useSsl, object callback)
        {
            NetTcpBinding clientBinding;

            if (useSsl)
            {
                clientBinding = new NetTcpBinding(SecurityMode.Transport);
                clientBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            }
            else
            {
                clientBinding = new NetTcpBinding(SecurityMode.None);
            }

            var channelFactory = new DuplexChannelFactory<TServer>(callback, clientBinding, uri);
            //TODO: Do i need to worry about that since this is ephemeral? -dkk
            //channelFactory.Closed += new EventHandler(DuplexChannelFactory_Closed);
            //channelFactory.Closing += new EventHandler(DuplexChannelFactory_Closing);
            //channelFactory.Faulted += new EventHandler(DuplexChannelFactory_Faulted);

            return channelFactory.CreateChannel();
        }
    }
}
