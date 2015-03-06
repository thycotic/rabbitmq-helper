using Thycotic.DistributedEngine.LogViewer.Models;

namespace Thycotic.Logging.LogTail
{
    /// <summary>
    /// Interface for a log tap
    /// </summary>
    public interface IRecentLogEntryProvider
    {
        /// <summary>
        /// Lists the recent events.
        /// </summary>
        /// <returns></returns>
        LogEntry[] GetEntries();

        /// <summary>
        /// Prunes this instance.
        /// </summary>
        void Clear();
    }
}