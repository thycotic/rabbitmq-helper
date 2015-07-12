using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Thycotic.Logging
{
    /// <summary>
    /// Log context
    /// </summary>
    public sealed class LogContext : IDisposable
    {
        /// <summary>
        /// The context name
        /// </summary>
        public const string ContextName = "Context";

        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly IDisposable _context;

        [DebuggerStepThrough]
        private LogContext(string name)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));

            Contract.Ensures(_context != null);

            Contract.Assume(log4net.ThreadContext.Stacks != null);

            var stack = log4net.ThreadContext.Stacks[ContextName];

            if (stack == null)
            {
                throw new ApplicationException("Could not retrieve context stack");
            }

            var context = stack.Push(name);

            if (context == null)
            {
                throw new ApplicationException("Log context could not be established");
            }

            Contract.Assume(context != null);

            _context = context;
            _stopwatch.Start();
        }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static LogContext Create(string name)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
            
            Contract.Ensures(Contract.Result<LogContext>() != null);

            var nameIndented = string.Format(CultureInfo.InvariantCulture, "> {0}", name);

            Contract.Assume(!string.IsNullOrWhiteSpace(nameIndented));

            return new LogContext(nameIndented);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
            _stopwatch.Stop();

            var log = Log.Get(typeof(Stopwatch));

            log.Debug(string.Format(CultureInfo.InvariantCulture, "[ElapsedMilliseconds: {0}]", _stopwatch.ElapsedMilliseconds));

            _context.Dispose();
        }
    }
}
