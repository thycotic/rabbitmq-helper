using Thycotic.Messages.Common;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.Messages.DE.Areas.PasswordChanging.Request
{
    /// <summary>
    /// Blocking password change (using privileged credentials) message
    /// </summary>
    public class BlockingPrivilegedPasswordChangeMessage : BlockingConsumableBase
    {
        /// <summary>
        /// Gets or sets the info required for the password change.
        /// </summary>
        public IPrivilegedPasswordChangerInfo OperationInfo { get; set; }
    }
}