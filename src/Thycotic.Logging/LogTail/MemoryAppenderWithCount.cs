using log4net.Appender;

namespace Thycotic.Logging.LogTail
{
    /// <summary>
    /// Memory appender with count property
    /// </summary>
    public class MemoryAppenderWithCount : MemoryAppender
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                lock (m_eventsList)
                {
                    return m_eventsList != null ? m_eventsList.Count : 0;
                }
            }
        }
    }
}