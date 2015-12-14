using Thycotic.Utility.TestChain;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Interface for a priority scheduler provider
    /// </summary>
    [UnitTestsRequired]
    public interface IPrioritySchedulerProvider
    {
        /// <summary>
        /// Highest
        /// </summary>
        PriorityScheduler Highest { get; }

        /// <summary>
        /// AboveNormal
        /// </summary>
        PriorityScheduler AboveNormal { get; }

        /// <summary>
        /// BelowNormal
        /// </summary>
        PriorityScheduler BelowNormal { get; }

        /// <summary>
        /// Lowest
        /// </summary>
        PriorityScheduler Lowest { get; }

        /// <summary>
        /// Normal
        /// </summary>
        PriorityScheduler Normal { get; }
    }
}