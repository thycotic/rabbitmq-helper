using System;

namespace Thycotic.DistributedEngine.LogViewer.Models
{
    /// <summary>
    /// Log entry
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the service role.
        /// </summary>
        /// <value>
        /// The service role.
        /// </value>
        public string ServiceRole { get; set; }

        /// <summary>
        /// Gets or sets the correlation.
        /// </summary>
        /// <value>
        /// The correlation.
        /// </value>
        public string Correlation { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the thread.
        /// </summary>
        /// <value>
        /// The thread.
        /// </value>
        public string Thread { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public string Logger { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public string Exception { get; set; }

    }
}