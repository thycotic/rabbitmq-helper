using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.POC.Request
{
    /// <summary>
    /// Hello world message
    /// </summary>
    public class HelloWorldMessage : BasicConsumableBase
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

    }
}
