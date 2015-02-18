using System;

namespace Thycotic.SecretServerEngine.Web.Common.Request
{
    /// <summary>
    /// Engine authentication request when an engine tries to manipulate contents of an exchange.
    /// </summary>
    public class EngineAuthenticationRequest
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

        /// <summary>
        /// Gets or sets the name of the exchange.
        /// </summary>
        /// <value>
        /// The name of the exchange.
        /// </value>
        public string ExchangeName { get; set; }
    }
}