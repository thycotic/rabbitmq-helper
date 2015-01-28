using Thycotic.MessageQueueClient;

namespace Thycotic.Messages.Areas.POC
{
    public class HelloWorldMessage : IConsumable
    {
        public int Version { get; set; }
        public int RetryCount { get; set; }
        public string Content { get; set; }
    }
}
