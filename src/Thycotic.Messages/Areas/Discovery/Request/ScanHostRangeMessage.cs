using Thycotic.Discovery.Core.Inputs;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Host Range Message
    /// </summary>
    public class ScanHostRangeMessage : ScanMessageBase, IBlockingConsumable
    {
        /// <summary>
        /// Scan Host Range Input
        /// </summary>
        public ScanHostRangeInput Input { get; set; }
    }
}
