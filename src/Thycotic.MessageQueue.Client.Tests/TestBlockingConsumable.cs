using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Tests
{
    public class TestBlockingConsumable : IBlockingConsumable
    {
        public int Version { get; private set; }
        public string Content { get; set; }
    }
}