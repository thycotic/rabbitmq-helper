using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    public class BlockingConsumableDummy : IBlockingConsumable
    {
        public int Version { get; set; }
    }
}