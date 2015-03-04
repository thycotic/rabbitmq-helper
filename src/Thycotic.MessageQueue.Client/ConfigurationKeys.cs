namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigurationKeys
    {
        /// <summary>
        /// The heartbeat interval
        /// </summary>
        public const string HeartbeatInterval = "Heartbeat.Interval";

        /// <summary>
        /// Pipeline
        /// </summary>
        public static class Pipeline
        {
            /// <summary>
            /// The queue type
            /// </summary>
            public const string QueueType = "Pipeline.QueueType";
        }

        /// <summary>
        /// Exchange
        /// </summary>
        public static class Exchange
        {
            /// <summary>
            /// The queue exchange
            /// </summary>
            public const string Name = "Exchange.Name";

            /// <summary>
            /// The symmetric key
            /// </summary>
            public const string SymmetricKey = "Exchange.SymmetricKey";

            /// <summary>
            /// The initialization vector
            /// </summary>
            public const string InitializationVector = "Exchange.InitializationVector";
        }

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
            public static string UseSsl = "RabbitMq.UseSSL";
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
            public static string UseSsl = "MemoryMq.UseSSL";

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
