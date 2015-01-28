using Thycotic.MessageQueueClient;

namespace Thycotic.Messages.Areas.POC.Request
{
    public class ThrowingRpcMessage : IConsumable
    {
        public int Version { get; set; }
        public int RetryCount { get; set; }
    }
}
