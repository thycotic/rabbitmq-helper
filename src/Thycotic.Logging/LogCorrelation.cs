using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Thycotic.Logging
{
    /// <summary>
    /// Log context
    /// </summary>
    public sealed class LogCorrelation : IDisposable
    {
        /// <summary>
        /// The correlation name
        /// </summary>
        public const string CorrelationName = "Correlation";
        private readonly IDisposable _context;

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; private set; }

        [DebuggerStepThrough]
        private LogCorrelation()
        {
            Contract.Ensures(_context != null);

            Id = Guid.NewGuid().ToString();

            Contract.Assume(log4net.ThreadContext.Stacks != null);

            var stack = log4net.ThreadContext.Stacks[CorrelationName];

            if (stack == null)
            {
                throw new ApplicationException("Could not retrieve correlation stack");
            }

            var context = stack.Push(Id);
            if (context == null)
            {
                throw new ApplicationException("Log context could not be established");
            }

            Contract.Assume(context != null);

            _context = context;

        }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static LogCorrelation Create()
        {
            Contract.Ensures(Contract.Result<LogCorrelation>() != null);

            return new LogCorrelation();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
