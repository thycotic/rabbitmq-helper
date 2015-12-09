using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;

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
            /// MemoryMq specific configuration keys
            /// </summary>
            public class MemoryMq 
            {
                /// <summary>
                /// The connection string
                /// </summary>
                public const string ConnectionString = "Pipeline.ConnectionString";

                /// <summary>
                /// The user name
                /// </summary>
                public const string UserName = "Pipeline.UserName";

                /// <summary>
                /// The password
                /// </summary>
                public const string Password = "Pipeline.Password";

                /// <summary>
                /// Whether or not to use SSL
                /// </summary>
                public static string UseSsl = "Pipeline.UseSsl";

                /// <summary>
                /// The thumbprint
                /// </summary>
                public static string Thumbprint = "Pipeline.Thumbprint";
            }

            /// <summary>
            /// RabbitMq specific configuration keys
            /// </summary>
            public class RabbitMq
            {
                /// <summary>
                /// The connection string
                /// </summary>
                public const string ConnectionString = "Pipeline.ConnectionString";

                /// <summary>
                /// The user name
                /// </summary>
                public const string UserName = "Pipeline.UserName";

                /// <summary>
                /// The password
                /// </summary>
                public const string Password = "Pipeline.Password";

                /// <summary>
                /// Whether or not to use SSL
                /// </summary>
                public static string UseSsl = "Pipeline.UseSsl";

                /// <summary>
                /// The thumbprint
                /// </summary>
                public static string Thumbprint = "Pipeline.Thumbprint";
            }

            /// <summary>
            /// MemoryMq specific configuration keys
            /// </summary>
            public class AzureServiceBus 
            {

                /// <summary>
                /// The connection string
                /// </summary>
                public const string ConnectionString = "Pipeline.ConnectionString";

            }
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