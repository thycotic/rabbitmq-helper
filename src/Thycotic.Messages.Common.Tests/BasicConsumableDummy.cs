namespace Thycotic.Messages.Common.Tests
{
    internal class BasicConsumableDummy : IBasicConsumable
    {
        public int Version { get; private set; }
        public int RetryCount { get; set; }
    }
}