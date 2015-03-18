using System.ServiceModel;

namespace Thycotic.Wcf
{
    /// <summary>
    /// HTTP channel factory
    /// </summary>
    public static class HttpChannelFactory
    {
        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <typeparam name="TServer">The type of the server.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <returns></returns>
        public static TServer CreateChannel<TServer>(string uri, bool useSsl)
        {
            BasicHttpBinding clientBinding;

            if (useSsl)
            {
                clientBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                clientBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }
            else
            {
                clientBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            }

            var channelFactory = new ChannelFactory<TServer>(clientBinding, uri);
            
            return channelFactory.CreateChannel();
        }

    }
}
