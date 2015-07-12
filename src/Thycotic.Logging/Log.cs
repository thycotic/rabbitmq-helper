using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Thycotic.Logging.LogTail;

namespace Thycotic.Logging
{
    /// <summary>
    /// Log
    /// </summary>
    public sealed class Log
    {
        private static ILogWriterFactory _logWriterFactory = new GenericLogWriterFactory();

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

            Contract.Assume(repository != null);
            Contract.Assume(repository.Root != null);

            repository.Root.AddAppender(new MemoryAppenderWithCount
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
        [DebuggerStepThrough]
        public static IEnumerable<IAppender> GetAppenders()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IAppender>>() != null);

            var repository = LogManager.GetRepository();

            Contract.Assume(repository != null);

            var appenders = repository.GetAppenders();

            if (appenders == null)
            {
                throw new ApplicationException("No appenders found");
            }

            Contract.Assume(appenders != null);

            return appenders;
        }


        /// <summary>
        /// Sets the log writer factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        [DebuggerStepThrough]
        public static void SetLogWriterFactory(ILogWriterFactory factory)
        {
            Contract.Requires<ArgumentNullException>(factory != null);

            _logWriterFactory = factory;
        }

        /// <summary>
        /// Gets log for the specified type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static ILogWriter Get(Type type)
        {
            Contract.Ensures(Contract.Result<ILogWriter>() != null);

            var writer = _logWriterFactory.GetLogWriter(type);

            if (writer == null)
            {
                throw new ApplicationException("Could not get log writer");
            }

            Contract.Assume(writer != null);

            return writer;
        }

        private class GenericLogWriterFactory : ILogWriterFactory
        {
            /// <summary>
            /// Gets the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            public ILogWriter GetLogWriter(Type type)
            {
                return new GenericLogWriter(type);
            }
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
                Contract.Ensures(_log2 != null);

                _log2 = LogManager.GetLogger(type);

                if (_log2 == null)
                {
                    throw new ApplicationException("Could not create underlying logger");
                }

                Contract.Assume(_log2 != null);
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