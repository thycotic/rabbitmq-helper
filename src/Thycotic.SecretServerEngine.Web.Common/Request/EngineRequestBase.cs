using System;

namespace Thycotic.SecretServerEngine.Web.Common.Request
{
    /// <summary>
    /// Base engine response
    /// </summary>
    public class EngineRequestBase
    {
        /// <summary>
        /// Gets or sets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        public Guid IdentityGuid { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public double Version { get; set; }

    }
}