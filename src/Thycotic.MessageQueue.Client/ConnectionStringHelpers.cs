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
            return GetMemoryMqConnectionString(DnsEx.GetDnsHostName(), portNumber);
        }

        /// <summary>
        /// Gets the local rabbit mq connection string.
        /// </summary>
        /// <param name="portNumber">The port number.</param>
        /// <returns></returns>
        public static string GetLocalRabbitMqConnectionString(int portNumber = DefaultPorts.RabbitMq.NonSsl)
        {
            return GetRabbitMqConnectionString(DnsEx.GetDnsHostName(), portNumber);
        }

        /// <summary>
        /// Gets the memory mq connection string.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="portNumber">The port number.</param>
        /// <returns></returns>
        public static string GetMemoryMqConnectionString(string hostName, int portNumber = DefaultPorts.MemoryMq.NonSsl)
        {
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
            return GetConnectionString(DefaultSchemas.RabbitMq, hostName, portNumber);
        }

        private static string GetConnectionString(string scheme, string hostName, int portNumber)
        {
            return string.Format("{0}://{1}:{2}", scheme, hostName, portNumber);
        }
    }
}
