using Thycotic.Discovery.Core.Inputs;

namespace Thycotic.Messages.DE.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Dependency Message
    /// </summary>
    public class ScanDependencyMessage : ScanMessageBase
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
