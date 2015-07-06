using System;
using System.Diagnostics.Contracts;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a blocking consumable
    /// </summary>
    [ContractClass(typeof(BasicConsumableContract))]
    public interface IBasicConsumable : IConsumable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IBasicConsumable"/> was redelivered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if redelivered; otherwise, <c>false</c>.
        /// </value>
        bool Redelivered { get; set; }

        /// <summary>
        /// Gets the expires on datetime in UTC.
        /// </summary>
        /// <value>
        /// The expires on.
        /// </value>
        DateTime? ExpiresOn { get;  }


        /// <summary>
        /// Gets a value indicating whether the consumable should be relayed even if it has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if relay even if expired; otherwise, <c>false</c>.
        /// </value>
        bool RelayEvenIfExpired { get; }
    }

    /// <summary>
    /// Contract for IBasicConsumable
    /// </summary>
    [ContractClassFor(typeof(IBasicConsumable))]
    public abstract class BasicConsumableContract : IBasicConsumable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IBasicConsumable"/> was redelivered.
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
        public DateTime? ExpiresOn { get; private set; }


        /// <summary>
        /// Gets a value indicating whether the consumable should be relayed even if it has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if relay even if expired; otherwise, <c>false</c>.
        /// </value>
        public bool RelayEvenIfExpired { get; private set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; private set; }
    }
}