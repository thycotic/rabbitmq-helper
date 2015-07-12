using System;
using System.Diagnostics.Contracts;
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
            Contract.Ensures(Contract.Result<IContextChannel>() != null);

            // ReSharper disable once SuspiciousTypeConversion.Global
            var contextChannel = callback as IContextChannel;

            if (contextChannel == null)
            {
                throw new ApplicationException("Unable to cast callback to context channel");
            }

            Contract.Assume(contextChannel != null);

            return contextChannel;
        }

        /// <summary>
        /// Gets the communication object.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static ICommunicationObject ToCommunicationObject(this IWcfService service)
        {

            Contract.Ensures(Contract.Result<ICommunicationObject>() != null);

            // ReSharper disable once SuspiciousTypeConversion.Global
            var communicationObject = service as ICommunicationObject;

            if (communicationObject == null)
            {
                throw new ApplicationException("Unable to cast service to communication object");
            }

            Contract.Assume(communicationObject != null);

            return communicationObject;
        }
    }
}
