namespace Thycotic.Messages.Common.Tests
{
    public class TestBlockingConsumable : IBlockingConsumable
    {
        public int Version { get; set; }
        public string Content { get; set; }
    }
}