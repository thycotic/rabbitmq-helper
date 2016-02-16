using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.POC.Request
{
    /// <summary>
    /// Slow RPC message
    /// </summary>
    public class StepMessage : BlockingConsumableBase
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public int Count { get; set; }
    }
}
