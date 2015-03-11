using Thycotic.AppCore.Federator;

namespace Thycotic.Messages.Heartbeat.Response
{
    /// <summary>
    /// Secret heartbeat response
    /// </summary>
    public class SecretHeartbeatResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SecretHeartbeatResponse"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error core.
        /// </summary>
        /// <value>
        /// The error core.
        /// </value>
        public FailureCode ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the status messages.
        /// </summary>
        /// <value>
        /// The status messages.
        /// </value>
        public string[] StatusMessages { get; set; }

        
    }
}
