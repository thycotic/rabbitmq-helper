using System.ServiceModel;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Memory mq client proxy
    /// </summary>
    public class MemoryMqServerClientProxy
    {
        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public IContextChannel Channel { get; private set; }

        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        public IMemoryMqServerCallback Callback { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServerClientProxy"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="callback">The callback.</param>
        public MemoryMqServerClientProxy(IContextChannel channel, IMemoryMqServerCallback callback)
        {
            Channel = channel;
            Callback = callback;
        }
    }
}