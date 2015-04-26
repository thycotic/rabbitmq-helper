namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigurationKeys
    {
        /// <summary>
        /// Engine
        /// </summary>
        public static class Engine
        {
            /// <summary>
            /// The symmetric key
            /// </summary>
            public const string SymmetricKey = "Engine.SymmetricKey";

            /// <summary>
            /// The initialization vector
            /// </summary>
            public const string InitializationVector = "Engine.InitializationVector";

            /// <summary>
            /// The heartbeat interval
            /// </summary>
            public const string HeartbeatIntervalSeconds = "Engine.Heartbeat.IntervalSeconds";
        }

        /// <summary>
        /// Pipeline
        /// </summary>
        public static class Pipeline
        {
            /// <summary>
            /// The queue type
            /// </summary>
            public const string QueueType = "Pipeline.QueueType";

            /// <summary>
            /// The connection string
            /// </summary>
            public const string ConnectionString = MemoryMq.ConfigurationKeys.ConnectionString;

            /// <summary>
            /// The user name
            /// </summary>
            public const string UserName = MemoryMq.ConfigurationKeys.UserName;

            /// <summary>
            /// The password
            /// </summary>
            public const string Password = MemoryMq.ConfigurationKeys.Password;

            /// <summary>
            /// Whether or not to use SSL
            /// </summary>
            public static string UseSsl = MemoryMq.ConfigurationKeys.UseSsl;
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

    }
}