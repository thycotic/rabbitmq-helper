namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Blocking consumer (remote procedure call) result used for normal operations
    /// </summary>
    public class BlockingConsumerResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the RPC call succeeded or failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public bool Status { get; set; }


        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        /// <value>
        /// The status text.
        /// </value>
        public string StatusText { get; set; }
    }
}
