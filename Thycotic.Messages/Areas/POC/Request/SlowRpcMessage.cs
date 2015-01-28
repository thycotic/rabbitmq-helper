using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.POC.Request
{
    public class SlowRpcMessage : IConsumable
    {
        public string[] Items { get; set; }
        public int Version { get; set; }
        public int RetryCount { get; set; }
    }
}
