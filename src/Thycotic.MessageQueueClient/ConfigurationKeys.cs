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

            /// <summary>
            /// The thumb print
            /// </summary>
            public const string Thumbprint = "MemoryMq.Thumbprint";

            /// <summary>
            /// The start server
            /// </summary>
            public const string StartServer = "MemoryMq.StartServer";
        }
    }
}
