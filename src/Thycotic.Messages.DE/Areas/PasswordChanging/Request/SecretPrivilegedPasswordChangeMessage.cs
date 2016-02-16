using Thycotic.Messages.Common;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.Messages.DE.Areas.PasswordChanging.Request
{
    /// <summary>
    /// Secret password change (using privileged credentials) message
    /// </summary>
    public class SecretPrivilegedPasswordChangeMessage : BasicConsumableBase
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
        /// Gets or sets the optional Secret Change Dependency Message.
        /// </summary>
        public SecretChangeDependencyMessage SecretChangeDependencyMessage { get; set; }
    }
}