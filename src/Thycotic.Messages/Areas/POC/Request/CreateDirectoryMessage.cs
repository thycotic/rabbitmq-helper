using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.POC.Request
{
    /// <summary>
    /// Chain message
    /// </summary>
    public class CreateDirectoryMessage : BlockingConsumableBase
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }
    }
}
