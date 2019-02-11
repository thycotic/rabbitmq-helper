using System.Threading;

namespace Thycotic.RabbitMq.Helper.Logic.Workflow
{
    /// <summary>
    /// Activity ID provider for progress operations.
    /// </summary>
    public static class ActivityIdProvider
    {
        private static int _globalActivityId = 1;

        /// <summary>
        /// Gets the next activity identifier.
        /// </summary>
        /// <returns></returns>
        public static int GetNextActivityId()
        {
            return Interlocked.Increment(ref _globalActivityId);
        }
    }
}