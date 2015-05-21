using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    public class BlockingConsumableDummy : IBlockingConsumable
    {
        public BlockingConsumableDummy(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }
    }
}