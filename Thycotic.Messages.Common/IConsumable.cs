namespace Thycotic.MessageQueueClient
{
    public interface IConsumable
    {
        int Version { get; }
        int RetryCount { get; set; }
    }
}
