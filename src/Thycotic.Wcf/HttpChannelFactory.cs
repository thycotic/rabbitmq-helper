using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Thycotic.Wcf
{
    /// <summary>
    /// HTTP channel factory
    /// </summary>
    public static class HttpChannelFactory
    {
        private static Binding GetBinding(bool useSsl, bool useEnvelopeAuth)
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

            if (useEnvelopeAuth)
            {
                clientBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            }

            return clientBinding;
        }

        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <typeparam name="TServer">The type of the server.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static TServer CreateChannel<TServer>(string uri, bool useSsl, string userName = "", string password = "")
        {
            var useEnvelopeAuth = !string.IsNullOrWhiteSpace(userName);

            var channelFactory = new ChannelFactory<TServer>(GetBinding(useSsl, useEnvelopeAuth), uri);

            if (useEnvelopeAuth && channelFactory.Credentials != null)
            {
                channelFactory.Credentials.UserName.UserName = userName;
                channelFactory.Credentials.UserName.Password = password;
            }

            return channelFactory.CreateChannel();
        }

    }
}
