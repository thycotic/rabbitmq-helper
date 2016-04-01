using Thycotic.Messages.Common;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.Messages.DE.Areas.PasswordChanging.Request
{
    /// <summary>
    /// BlockingS password change (using the account's credentials) message
    /// </summary>
    public class BlockingPasswordChangeMessage : BlockingConsumableBase
    {
        /// <summary>
        /// Gets or sets the info required for the password change.
        /// </summary>
        public IBasicPasswordChangerInfo OperationInfo { get; set; }
    }
}