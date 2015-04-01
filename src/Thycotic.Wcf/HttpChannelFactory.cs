using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Thycotic.Logging;

namespace Thycotic.Wcf
{
    /// <summary>
    /// HTTP channel factory
    /// </summary>
    public static class HttpChannelFactory
    {
        private static readonly ILogWriter Log = Logging.Log.Get(typeof(HttpChannelFactory));
        
        private static Binding GetBinding(bool useSsl, bool useEnvelopeAuth)
        {
            BasicHttpBinding clientBinding;

            if (useSsl)
            {
                clientBinding = new BasicHttpBinding(useEnvelopeAuth ? BasicHttpSecurityMode.TransportWithMessageCredential : BasicHttpSecurityMode.Transport);
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

            if (useSsl)
            {
                if (useEnvelopeAuth)
                {
                    if (channelFactory.Credentials == null)
                    {
                        throw new InvalidOperationException("No credentials available");
                    }

                    channelFactory.Credentials.UserName.UserName = userName;
                    channelFactory.Credentials.UserName.Password = password;
                }
            }
            else
            {
                Log.Warn("Channel will not send client credentials. Use SSL for increased security");
            }

            return channelFactory.CreateChannel();
        }

    }
}
