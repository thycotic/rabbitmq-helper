using Thycotic.Discovery.Core.Inputs;

namespace Thycotic.Messages.DE.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Host Range Message
    /// </summary>
    public class ScanHostRangeMessage : ScanMessageBase
    {
        /// <summary>
        /// Scan Host Range Input
        /// </summary>
        public ScanHostRangeInput Input { get; set; }
    }
}
