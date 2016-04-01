using Thycotic.Messages.Common;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.Messages.DE.Areas.Verify.Request
{
    /// <summary>
    /// Verify A Username and Password
    /// </summary>
    public class VerifyPasswordMessage : BlockingConsumableBase
    {
       /// <summary>
        /// Gets or sets the password information provider.
        /// </summary>
        /// <value>
        /// The password information provider.
        /// </value>
        public IVerifyCredentialsInfo VerifyCredentialsInfo { get; set; }
    }
}
