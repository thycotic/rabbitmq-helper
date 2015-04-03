using Thycotic.Logging.Models;

namespace Thycotic.Logging.LogTail
{
    /// <summary>
    /// Interface for a log tap
    /// </summary>
    public interface IRecentLogEntryProvider
    {
        /// <summary>
        /// Gets or the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }

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