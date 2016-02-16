using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.POC.Request
{
    /// <summary>
    /// Chain message
    /// </summary>
    public class CreateFileMessage : BasicConsumableBase
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string FileName { get; set; }
    }
}
