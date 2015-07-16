using Thycotic.Discovery.Core.Inputs;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Dependency Message
    /// </summary>
    public class ScanDependencyBlockingMessage : ScanDependencyMessage, IBlockingConsumable
    {
    }
}
