using System;
using System.Diagnostics.Contracts;
using System.Linq;
using log4net.Core;
using Thycotic.Logging.Models;

namespace Thycotic.Logging.LogTail
{
    /// <summary>
    /// Log tap to get log events
    /// </summary>
    public class RecentLogEntryProvider : IRecentLogEntryProvider
    {
        private readonly Lazy<MemoryAppenderWithCount> _appender;

        private readonly ILogWriter _log = Log.Get(typeof(RecentLogEntryProvider));

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentLogEntryProvider"/> class.
        /// </summary>
        public RecentLogEntryProvider()
        {
            _log.Debug("Attaching memory appender");

            _appender =
                new Lazy<MemoryAppenderWithCount>(
                    () =>
                        (MemoryAppenderWithCount)
                            Log.GetAppenders()
                                .SingleOrDefault(a => a is MemoryAppenderWithCount && a.Name == BuiltInLogNames.RecentEventsMemoryAppender));
        }

        private MemoryAppenderWithCount ActualAppender
        {
            get
            {
                var actualAppender = _appender.Value;

                if (actualAppender == null)
                {
                    throw new ApplicationException("No recent log appender found");
                }

                Contract.Assume(actualAppender != null);

                return actualAppender;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return ActualAppender.Count; }
        }

        /// <summary>
        /// Lists the recent events.
        /// </summary>
        /// <returns></returns>
        public LogEntry[] GetEntries()
        {
            var events = ActualAppender.GetEvents();

            if (events == null)
            {
                throw new ApplicationException("No events enumerable retrieved");
            }

            return events.ToArray().Select(Map).ToArray();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _log.Debug("Clearing log");

            ActualAppender.Clear();
        }

        private static LogEntry Map(LoggingEvent loggingEvent)
        {
            return new LogEntry
            {
                Date = loggingEvent.TimeStamp,
                UserId = loggingEvent.UserName,
                Logger = loggingEvent.LoggerName,
                Correlation = loggingEvent.Properties.Contains(LogCorrelation.CorrelationName) ? (string)loggingEvent.Properties[LogCorrelation.CorrelationName] : string.Empty,
                Context = loggingEvent.Properties.Contains(LogContext.ContextName) ? (string)loggingEvent.Properties[LogContext.ContextName] : string.Empty,
                Thread = loggingEvent.ThreadName,
                Level = loggingEvent.Level != null ? loggingEvent.Level.DisplayName : string.Empty,
                Message = loggingEvent.RenderedMessage,
                Exception = loggingEvent.GetExceptionString()
            };
        }

    }
}
