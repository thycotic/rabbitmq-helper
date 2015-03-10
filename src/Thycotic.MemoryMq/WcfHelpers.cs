using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Channel helpers, mostly for WCF casting
    /// </summary>
    public static class WcfHelpers
    {
        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static IContextChannel ToContextChannel(this IMemoryMqWcfServerCallback callback)
        {
// ReSharper disable once SuspiciousTypeConversion.Global
            return (IContextChannel) callback;
        }

        /// <summary>
        /// Gets the communication object.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static ICommunicationObject ToCommunicationObject(this IMemoryMqWcfServer server)
        {
// ReSharper disable once SuspiciousTypeConversion.Global
            return (ICommunicationObject) server;
        }
    }
}
