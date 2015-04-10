using Thycotic.Messages.Common;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.Messages.PasswordChanging.Request
{
    /// <summary>
    /// Secret password change (using privileged credentials) message
    /// </summary>
    public class SecretPrivilegedPasswordChangeMessage : IBasicConsumable
    {
        /// <summary>
        /// Gets or sets the info required for the password change.
        /// </summary>
        public IPrivilegedPasswordChangerInfo OperationInfo { get; set; }

        /// <summary>
        /// Gets or sets the Secret Id
        /// </summary>
        public int SecretId { get; set; }

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