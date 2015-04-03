using System;
using System.Diagnostics;

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
            Id = Guid.NewGuid().ToString();

            _context = log4net.ThreadContext.Stacks[CorrelationName].Push(Id);
        }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static LogCorrelation Create()
        {
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
