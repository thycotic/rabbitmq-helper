using System.ServiceModel;

namespace Thycotic.MemoryMq.Subsystem
{
    public class MemoryMqServerClient
    {
        public IContextChannel Channel { get; set; }
        public IMemoryMqServerCallback Callback { get; set; }
    }
}