using Thycotic.Discovery.Core.Inputs;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Machine Message
    /// </summary>
    public class ScanMachineMessage : ScanMessageBase
    {
        /// <summary>
        /// Scan Machines Input
        /// </summary>
        public ScanMachinesInput Input { get; set; }
    }
}