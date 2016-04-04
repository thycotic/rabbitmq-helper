using System.Collections.Generic;
using Thycotic.SharedTypes.PasswordChangers;

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

        /// <summary>
        /// Command Execution Results
        /// </summary>
        public IReadOnlyList<CommandExecutionResult> CommandExecutionResults { get; set; }
    }
}
