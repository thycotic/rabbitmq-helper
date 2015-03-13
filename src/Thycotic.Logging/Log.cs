using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace Thycotic.Logging
{
    /// <summary>
    /// Log
    /// </summary>
    public sealed class Log
    {

        private static readonly ILogWriter LogInternal = Get(typeof(Log));

        /// <summary>
        /// Configures this instance.
        /// </summary>
        [DebuggerStepThrough]
        public static void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();

        }

        /// <summary>
        /// Attaches the memory appender.
        /// </summary>
        [DebuggerStepThrough]
        public static void AttachRecentEventsMemoryAppender()
        {
            var repository = (Hierarchy)LogManager.GetRepository();
            repository.Root.AddAppender(new MemoryAppender
            {
                Name = BuiltInLogNames.RecentEventsMemoryAppender
            });
        }

        /// <summary>
        /// Flushes all configured log appenders
        /// </summary>
        [DebuggerStepThrough]
        public static void Flush()
        {
            LogInternal.Debug(string.Format(CultureInfo.InvariantCulture, "Flushing logs"));

            GetAppenders().ToList().ForEach(a =>
            {
                if (!(a is BufferingAppenderSkeleton)) return;

                LogInternal.Debug(string.Format(CultureInfo.InvariantCulture, "Flushing {0}", a));

                var buffered = a as BufferingAppenderSkeleton;
                buffered.Flush();
            });

            LogInternal.Debug(string.Format(CultureInfo.InvariantCulture, "Flushed successfully"));

        }

        /// <summary>
        /// Gets the appenders.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IAppender> GetAppenders()
        {
            return LogManager.GetRepository().GetAppenders();
        }

        /// <summary>
        /// Gets log for the specified type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static ILogWriter Get(Type type)
        {
            return new GenericLogWriter(type);
        }

        /// <summary>
        /// Generic log writer
        /// </summary>
        private class GenericLogWriter : ILogWriter
        {
            private readonly ILog _log2;

            [DebuggerStepThrough]
            public GenericLogWriter(Type type)
            {
                _log2 = LogManager.GetLogger(type);
            }

            /// <summary>
            /// Logs a message with Debug level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Debug(string message)
            {
                _log2.Debug(message);
            }

            /// <summary>
            /// Logs a message with Debug level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Debug(string message, Exception exception)
            {
                _log2.Debug(message, exception);
            }

            /// <summary>
            /// Logs a message with Info level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Info(string message)
            {
                _log2.Info(message);
            }

            /// <summary>
            /// Logs a message with Info level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Info(string message, Exception exception)
            {
                _log2.Info(message, exception);
            }

            /// <summary>
            /// Logs a message with Warn level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Warn(string message)
            {
                _log2.Warn(message);
            }

            /// <summary>
            /// Logs a message with Warn level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Warn(string message, Exception exception)
            {
                _log2.Warn(message, exception);
            }

            /// <summary>
            /// Logs a message with Error level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Error(string message)
            {
                _log2.Error(message);
            }

            /// <summary>
            /// Logs a message with Error level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Error(string message, Exception exception)
            {
                _log2.Error(message, exception);
            }
        }
    }
}