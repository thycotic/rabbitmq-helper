namespace Thycotic.Messages.Common.Tests
{
    internal class BasicConsumableDummy : IBasicConsumable
    {
        public int Version { get; set; }
        public int RetryCount { get; set; }
    }
}