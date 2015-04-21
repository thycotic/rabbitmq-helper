using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.POC.Request
{
    /// <summary>
    /// Slow RPC message
    /// </summary>
    public class SortListMessage : BlockingConsumableBase
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public string[] Items { get; set; }
    }
}
