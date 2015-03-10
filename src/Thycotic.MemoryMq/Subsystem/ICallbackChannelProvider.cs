namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a callback channel provider
    /// </summary>
    public interface ICallbackChannelProvider
    {
        /// <summary>
        /// Gets the callback channel.
        /// </summary>
        /// <returns></returns>
        IMemoryMqWcfServerCallback GetCallbackChannel();
    }
}