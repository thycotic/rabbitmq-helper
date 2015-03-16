using Thycotic.Wcf;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Memory mq WCF server wrapper.
    /// </summary>
    public class MemoryMqServiceHost : ServiceHostBase<MemoryMqWcfService, IMemoryMqWcfService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServiceHost"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MemoryMqServiceHost(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServiceHost" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        public MemoryMqServiceHost(string connectionString, string thumbprint) : base(connectionString, thumbprint)
        {
        }
    }
}