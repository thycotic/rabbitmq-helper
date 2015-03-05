namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Default message queue ports
    /// </summary>
    public static class DefaultPorts
    {
        /// <summary>
        /// Memory Mq
        /// </summary>
        public static class MemoryMq
        {
            /// <summary>
            /// The non SSL
            /// </summary>
            public const int NonSsl = 8672;

            /// <summary>
            /// The SSL
            /// </summary>
            public const int Ssl = 8671;
        }

        /// <summary>
        /// Rabbit Mq
        /// </summary>
        public static class RabbitMq
        {
            /// <summary>
            /// The non SSL
            /// </summary>
            public const int NonSsl = 5672;

            /// <summary>
            /// The SSL
            /// </summary>
            public const int Ssl = 5671;
        }
    }
}
