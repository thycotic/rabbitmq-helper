using System;
using System.Linq;
using log4net.Appender;
using log4net.Core;
using Thycotic.Logging.Models;

namespace Thycotic.Logging.LogTail
{
    /// <summary>
    /// Log tap to get log events
    /// </summary>
    public class RecentLogEntryProvider : IRecentLogEntryProvider
    {
        private readonly Lazy<MemoryAppender> _appender; 

        private readonly ILogWriter _log = Log.Get(typeof(RecentLogEntryProvider));

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentLogEntryProvider"/> class.
        /// </summary>
        public RecentLogEntryProvider()
        {
            _log.Debug("Attaching memory appender");

            _appender =
                new Lazy<MemoryAppender>(
                    () =>
                        (MemoryAppender)
                            Log.GetAppenders()
                                .SingleOrDefault(a => a is MemoryAppender && a.Name == BuiltInLogNames.RecentEventsMemoryAppender));
        }

        /// <summary>
        /// Lists the recent events.
        /// </summary>
        /// <returns></returns>
        public LogEntry[] GetEntries()
        {
            return _appender.Value.GetEvents().ToArray().Select(Map).ToArray();
        }
        
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _log.Debug("Clearing log");

            _appender.Value.Clear();
        }

        private static LogEntry Map(LoggingEvent loggingEvent)
        {
            return new LogEntry
            {
                Date = loggingEvent.TimeStamp,
                Level = loggingEvent.Level.DisplayName,
                Message = loggingEvent.RenderedMessage,
                Exception = loggingEvent.GetExceptionString()
            };
        }

    }
}
