namespace Thycotic.Messages.Common.Tests
{
    internal class BlockingConsumableDummy : IBlockingConsumable
    {
        public int Version { get; set; }
        public int RetryCount { get; set; }
    }
}