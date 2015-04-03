using Thycotic.Discovery.Core.Inputs;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Dependency Message
    /// </summary>
    public class ScanDependencyMessage : ScanMessageBase
    {
        /// <summary>
        /// Scan Computer Input
        /// </summary>
        public ScanComputerInput Input { get; set; }
    }
}
