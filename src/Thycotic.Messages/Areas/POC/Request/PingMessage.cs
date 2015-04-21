using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.POC.Request
{
    /// <summary>
    /// Hello world message
    /// </summary>
    public class PingMessage : BasicConsumableBase
    {

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence { get; set; }
    }
}
