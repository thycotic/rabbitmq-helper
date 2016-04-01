using System.Collections.Generic;

namespace Thycotic.Messages.DE.Areas.Verify.Response
{
    /// <summary>
    /// Verify Password Response
    /// </summary>
    public class VerifyPasswordResponse
    {
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public IReadOnlyList<SharedTypes.PasswordChangers.Error> Errors { get; set; }
    }
}
