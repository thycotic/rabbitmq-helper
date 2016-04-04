using System.Collections.Generic;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.Messages.DE.Areas.PasswordChanging.Response
{
    /// <summary>
    /// Blocking Password Change response
    /// </summary>
    public class BlockingPasswordChangeResponse
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
