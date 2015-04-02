using Thycotic.AppCore.Federator;

namespace Thycotic.Messages.Areas.PasswordChanging.Response
{
    /// <summary>
    /// Secret heartbeat response
    /// </summary>
    public class SecretChangePasswordResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SecretChangePasswordResponse"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the Secret Id
        /// </summary>
        public int SecretId { get; set; }

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

        /// <summary>
        /// Gets or sets the command execution results.
        /// </summary>
        /// <value>
        /// The command execution results.
        /// </value>
        public CommandExecutionResult[] CommandExecutionResults { get; set; }
    }
}
