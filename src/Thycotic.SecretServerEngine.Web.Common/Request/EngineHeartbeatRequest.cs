using System;

namespace Thycotic.SecretServerEngine.Web.Common.Request
{
    /// <summary>
    /// Engine authentication request when an engine tries to manipulate contents of an exchange.
    /// </summary>
    public class EngineHeartbeatRequest : EngineRequestBase
    {
        /// <summary>
        /// Gets or sets the last activity.
        /// </summary>
        /// <value>
        /// The last activity.
        /// </value>
        public DateTime LastActivity { get; set; }
    }
}