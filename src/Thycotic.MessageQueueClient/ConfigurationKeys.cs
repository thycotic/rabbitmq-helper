using System;

namespace Thycotic.MessageQueueClient
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigurationKeys
    {
        /// <summary>
        /// The queue type
        /// </summary>
        public const string QueueType = "Queue.Type";

        /// <summary>
        /// The queue exchange
        /// </summary>
        public const string QueueExchangeName = "Queue.ExchangeName";

        /// <summary>
        /// Rabbit Mq
        /// </summary>
        public static class RabbitMq
        {
            /// <summary>
            /// The connection string
            /// </summary>
            public const string ConnectionString = "RabbitMq.ConnectionString";

            /// <summary>
            /// The user name
            /// </summary>
            public const string UserName = "RabbitMq.UserName";

            /// <summary>
            /// The password
            /// </summary>
            public const string Password = "RabbitMq.Password";

            /// <summary>
            /// Whether or not to use SSL
            /// </summary>
            public static string UseSSL = "RabbitMq.UseSSL";
        }

        /// <summary>
        /// Memory Mq
        /// </summary>
        public static class MemoryMq
        {
            /// <summary>
            /// The connection string
            /// </summary>
            public const string ConnectionString = "MemoryMq.ConnectionString";

            //TODO: Use username/password? -dkk

            /// <summary>
            /// Whether or not to use SSL
            /// </summary>
            public static string UseSSL = "MemoryMq.UseSSL";

            /// <summary>
            /// Server related
            /// </summary>
            public static class Server
            {

                /// <summary>
                /// The thumb print
                /// </summary>
                public const string Thumbprint = "MemoryMq.Server.Thumbprint";

                /// <summary>
                /// The start server
                /// </summary>
                public const string Start = "MemoryMq.Server.Start";

            }
        }
    }
}
