using System;
using System.Management.Automation;
using System.Security.Authentication;
using RabbitMQ.Client;

namespace Thycotic.RabbitMq.Helper.PSCommands
{
    /// <summary>
    /// Connection extensions
    /// </summary>
    public static class ConnectionExtensions
    {

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="hostname">The hostname.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="useTls">if set to <c>true</c> [use TLS].</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static IConnection GetConnection(this Cmdlet cmdlet, string hostname, string userName, string password, bool useTls, int? port = null)
        {

            const int nonTlsPort = 5672;
            const int tlsPort = 5671;
            
            var uriBuild = new UriBuilder
            {
                Scheme = "amqp",
                Host = hostname,
                Port = port ?? (useTls ? tlsPort : nonTlsPort)
            };

            var uri = uriBuild.Uri;

                     cmdlet.WriteVerbose($"Getting connection for {uri}");
 

            var connectionFactory = new ConnectionFactory
            {
                Uri = uri,
                RequestedHeartbeat = 300,
                UserName = userName,
                Password = password
            };

            if (!useTls)
            {
                return connectionFactory.CreateConnection();
            }

            connectionFactory.Ssl = new SslOption
            {
                Enabled = true,
                ServerName = uri.Host,
                Version = SslProtocols.Tls11 | SslProtocols.Tls12
            };

            return connectionFactory.CreateConnection();
        }
    }
}
