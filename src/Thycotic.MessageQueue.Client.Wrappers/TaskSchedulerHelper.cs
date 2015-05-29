using System.Threading;
using System.Threading.Tasks;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Task scheduler helper
    /// </summary>
    public static class TaskSchedulerHelper
    {
        /// <summary>
        /// Returns a task scheduler from the current synchronization context. A context is created if it doesn't exist already
        /// </summary>
        /// <returns></returns>
        public static TaskScheduler FromCurrentSynchronizationContext()
        {
            EnsureSynchronizationContextExists();

            return TaskScheduler.FromCurrentSynchronizationContext();
        }


        /// <summary>
        /// Ensures the synchronization context exists.
        /// </summary>
        private static void EnsureSynchronizationContextExists()
        {
            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }
        }
    }
}