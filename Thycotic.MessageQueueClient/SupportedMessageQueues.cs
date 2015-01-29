using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.MessageQueueClient
{
    /// <summary>
    /// Supported message queue/queue types
    /// </summary>
    public static class SupportedMessageQueues
    {
        /// <summary>
        /// Alias for rabbit message queue
        /// </summary>
        public const string RabbitMq = "RabbitMq";

        /// <summary>
        /// Alias for memory message queue
        /// </summary>
        public const string MemoryMq = "MemoryMq";
    }
}
