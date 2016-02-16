using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Dependency Message
    /// </summary>
    public class ScanDependencyBlockingMessage : ScanDependencyMessage, IBlockingConsumable
    {
    }
}
