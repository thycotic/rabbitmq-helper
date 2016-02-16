using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.Connectivity.Request
{
    /// <summary>
    /// Hello world message
    /// </summary>
    public class PingMessage : BlockingConsumableBase
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
