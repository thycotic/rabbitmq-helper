using System.ServiceModel;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Channel helpers, mostly for WCF casting
    /// </summary>
    public static class CastHelpers
    {
        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static IContextChannel ToContextChannel(this IWcfServerCallback callback)
        {
// ReSharper disable once SuspiciousTypeConversion.Global
            return (IContextChannel) callback;
        }

        /// <summary>
        /// Gets the communication object.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static ICommunicationObject ToCommunicationObject(this IWcfService service)
        {
// ReSharper disable once SuspiciousTypeConversion.Global
            return (ICommunicationObject) service;
        }
    }
}
