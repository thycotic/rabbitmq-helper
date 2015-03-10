using System;
using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Memory mq client proxy
    /// </summary>
    //TODO: Do away with this totally useless class -dkk
    public class MemoryMqWcfServerClientProxy
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
        public IMemoryMqWcfServerCallback Callback { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqWcfServerClientProxy"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="callback">The callback.</param>
        public MemoryMqWcfServerClientProxy(IContextChannel channel, IMemoryMqWcfServerCallback callback)
        {
            Channel = channel;
            Callback = callback;
        }
    }
}