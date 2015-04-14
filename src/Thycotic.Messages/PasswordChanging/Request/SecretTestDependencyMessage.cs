﻿using Thycotic.Messages.Common;
using Thycotic.SharedTypes.Dependencies;

namespace Thycotic.Messages.PasswordChanging.Request
{
    /// <summary>
    /// Secret heartbeat message
    /// </summary>
    public class SecretTestDependencyMessage : IBlockingConsumable
    {
        /// <summary>
        /// Gets or sets the dependency changer infos.
        /// </summary>
        /// <value>
        /// The dependencies and what to change them with.
        /// </value>
        public IDependencyChangeInfo DependencyChangeInfo { get; set; }

        /// <summary>
        /// Gets or sets the Secret Id
        /// </summary>
        public int SecretId { get; set; }

        /// <summary>
        /// Gets or sets the WMI Timeout
        /// </summary>
        public int WmiTimeout { get; set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        public int RetryCount { get; set; }
    }
}