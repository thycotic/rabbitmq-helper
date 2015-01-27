using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using log4net.Appender;

namespace Thycotic.Logging
{
    /// <summary>
    /// Log
    /// </summary>
    public sealed class Log
    {

        private static readonly ILogWriter _log = Get(typeof(Log));

        /// <summary>
        /// Configures this instance.
        /// </summary>
        [DebuggerStepThrough]
        public static void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Flushes all configured log appenders
        /// </summary>
        [DebuggerStepThrough]
        public static void Flush()
        {
            _log.Debug(string.Format(CultureInfo.InvariantCulture, "Flushing logs"));

            GetAppenders().ToList().ForEach(a =>
            {
                if (!(a is BufferingAppenderSkeleton)) return;

                _log.Debug(string.Format(CultureInfo.InvariantCulture, "Flushing {0}", a));

                var buffered = a as BufferingAppenderSkeleton;
                buffered.Flush();
            });

            _log.Debug(string.Format(CultureInfo.InvariantCulture, "Flushed successfully"));

        }

        private static IEnumerable<IAppender> GetAppenders()
        {
            return log4net.LogManager.GetRepository().GetAppenders();
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
            private readonly log4net.ILog _log;

            [DebuggerStepThrough]
            public GenericLogWriter(Type type)
            {
                _log = log4net.LogManager.GetLogger(type);
            }

            /// <summary>
            /// Logs a message with Debug level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Debug(string message)
            {
                _log.Debug(message);
            }

            /// <summary>
            /// Logs a message with Debug level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Debug(string message, Exception exception)
            {
                _log.Debug(message, exception);
            }

            /// <summary>
            /// Logs a message with Info level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Info(string message)
            {
                _log.Info(message);
            }

            /// <summary>
            /// Logs a message with Info level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Info(string message, Exception exception)
            {
                _log.Info(message, exception);
            }

            /// <summary>
            /// Logs a message with Warn level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Warn(string message)
            {
                _log.Warn(message);
            }

            /// <summary>
            /// Logs a message with Warn level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Warn(string message, Exception exception)
            {
                _log.Warn(message, exception);
            }

            /// <summary>
            /// Logs a message with Error level.
            /// </summary>
            /// <param name="message">The message.</param>
            [DebuggerStepThrough]
            public void Error(string message)
            {
                _log.Error(message);
            }

            /// <summary>
            /// Logs a message with Error level.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="exception">The exception.</param>
            [DebuggerStepThrough]
            public void Error(string message, Exception exception)
            {
                _log.Error(message, exception);
            }
        }
    }
}