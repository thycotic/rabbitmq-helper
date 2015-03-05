namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Default message queue ports
    /// </summary>
    public static class DefaultSchemas
    {
        /// <summary>
        /// The memory mq schema
        /// </summary>
        public const string MemoryMq = "net.tcp";

        /// <summary>
        /// The rabbit mq schema
        /// </summary>
        public const string RabbitMq = "amqp";
    }
}
