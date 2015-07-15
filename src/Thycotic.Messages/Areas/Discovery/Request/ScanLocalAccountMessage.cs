using Thycotic.Discovery.Core.Inputs;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Local Account Message
    /// </summary>
    public class ScanLocalAccountMessage : ScanMessageBase
    {
        /// <summary>
        /// Computer Id
        /// </summary>
        public int ComputerId { get; set; }


        /// <summary>
        /// Scan Computer Input
        /// </summary>
        public ScanComputerInput Input { get; set; }
    }
}
