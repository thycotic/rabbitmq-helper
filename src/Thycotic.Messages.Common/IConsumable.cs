using System;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a consumable
    /// </summary>
    public interface IConsumable
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        [Obsolete("Not yet supported")]
        int RetryCount { get; set; }
    }
}
