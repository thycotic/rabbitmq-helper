using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.POC.Request
{
    /// <summary>
    /// Chain message
    /// </summary>
    public class CreateFileMessage : IConsumable
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string FileName { get; set; }

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
