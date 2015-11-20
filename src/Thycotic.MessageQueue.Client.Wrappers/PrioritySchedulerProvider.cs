using System.Threading;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Priority scheduler provider
    /// </summary>
    public class PrioritySchedulerProvider : IPrioritySchedulerProvider
    {
        /// <summary>
        /// Highest
        /// </summary>
        public PriorityScheduler Highest { get; set; }
        /// <summary>
        /// AboveNormal
        /// </summary>
        public PriorityScheduler AboveNormal { get; set; }
        /// <summary>
        /// BelowNormal
        /// </summary>
        public PriorityScheduler BelowNormal { get; set; }
        /// <summary>
        /// Lowest
        /// </summary>
        public PriorityScheduler Lowest { get; set; }
        /// <summary>
        /// Normal
        /// </summary>
        public PriorityScheduler Normal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritySchedulerProvider"/> class.
        /// </summary>
        /// <param name="syncContext"></param>
        public PrioritySchedulerProvider(SynchronizationContext syncContext)
        {
            Highest = new PriorityScheduler(syncContext, ThreadPriority.Highest);
            AboveNormal = new PriorityScheduler(syncContext, ThreadPriority.AboveNormal);
            BelowNormal = new PriorityScheduler(syncContext, ThreadPriority.BelowNormal);
            Lowest = new PriorityScheduler(syncContext, ThreadPriority.Lowest);
            Normal = new PriorityScheduler(syncContext, ThreadPriority.Normal);
        }
    }
}