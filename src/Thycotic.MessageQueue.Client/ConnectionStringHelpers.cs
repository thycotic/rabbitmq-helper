using System;
using System.Diagnostics.Contracts;
using Thycotic.Utility;

namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Connection string helpers
    /// </summary>
    public static class ConnectionStringHelpers
    {
        /// <summary>
        /// Gets the local memory mq connection string.
        /// </summary>
        /// <param name="portNumber">The port number.</param>
        /// <returns></returns>
        public static string GetLocalMemoryMqConnectionString(int portNumber = DefaultPorts.MemoryMq.NonSsl)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var hostName = DnsEx.GetDnsHostName();

            Contract.Assume(!string.IsNullOrWhiteSpace(hostName));

            return GetMemoryMqConnectionString(hostName, portNumber);
        }

        /// <summary>
        /// Gets the local rabbit mq connection string.
        /// </summary>
        /// <param name="portNumber">The port number.</param>
        /// <returns></returns>
        public static string GetLocalRabbitMqConnectionString(int portNumber = DefaultPorts.RabbitMq.NonSsl)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var hostName = DnsEx.GetDnsHostName();

            Contract.Assume(!string.IsNullOrWhiteSpace(hostName));

            return GetRabbitMqConnectionString(hostName, portNumber);
        }

        /// <summary>
        /// Gets the memory mq connection string.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="portNumber">The port number.</param>
        /// <returns></returns>
        public static string GetMemoryMqConnectionString(string hostName, int portNumber = DefaultPorts.MemoryMq.NonSsl)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(hostName));

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));


            return GetConnectionString(DefaultSchemas.MemoryMq, hostName, portNumber);
        }

        /// <summary>
        /// Gets the rabbit mq connection string.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="portNumber">The port number.</param>
        /// <returns></returns>
        public static string GetRabbitMqConnectionString(string hostName, int portNumber = DefaultPorts.RabbitMq.NonSsl)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(hostName));
            
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            return GetConnectionString(DefaultSchemas.RabbitMq, hostName, portNumber);
        }

        private static string GetConnectionString(string scheme, string hostName, int portNumber)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(scheme));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(hostName));

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var connectionString = string.Format("{0}://{1}:{2}", scheme, hostName, portNumber);

            Contract.Assume(connectionString != null);
            Contract.Assume(!string.IsNullOrWhiteSpace(connectionString));

            return connectionString;
        }
    }
}
