using System;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Basic consumable base
    /// </summary>
    public abstract class BasicConsumableBase : IBasicConsumable
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IBasicConsumable" /> was redelivered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if redelivered; otherwise, <c>false</c>.
        /// </value>
        public bool Redelivered { get; set; }

        /// <summary>
        /// Gets the expires on datetime in UTC.
        /// </summary>
        /// <value>
        /// The expires on.
        /// </value>
        public DateTime? ExpiresOn { get; set; }

        /// <summary>
        /// Gets a value indicating whether the consumable should be relayed even if it has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if relay even if expired; otherwise, <c>false</c>.
        /// </value>
        public bool RelayEvenIfExpired { get; set; }
    }
}