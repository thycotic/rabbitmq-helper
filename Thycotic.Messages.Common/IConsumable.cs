namespace Thycotic.Messages.Common
{
    public interface IConsumable
    {
        int Version { get; }
        int RetryCount { get; set; }
    }
}
