namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Interface for a priority scheduler provider
    /// </summary>
    public interface IPrioritySchedulerProvider
    {
        /// <summary>
        /// Highest
        /// </summary>
        PriorityScheduler Highest { get; set; }

        /// <summary>
        /// AboveNormal
        /// </summary>
        PriorityScheduler AboveNormal { get; set; }

        /// <summary>
        /// BelowNormal
        /// </summary>
        PriorityScheduler BelowNormal { get; set; }

        /// <summary>
        /// Lowest
        /// </summary>
        PriorityScheduler Lowest { get; set; }

        /// <summary>
        /// Normal
        /// </summary>
        PriorityScheduler Normal { get; set; }
    }
}