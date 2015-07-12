using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Callback channel provider
    /// </summary>
    public class CallbackChannelProvider : ICallbackChannelProvider
    {
        /// <summary>
        /// Gets the callback channel using OperationContext.Current.
        /// </summary>
        /// <returns></returns>
        public TCallback GetCallbackChannel<TCallback>()
        {
            if (OperationContext.Current == null)
            {
                throw new ApplicationException("No operation context available");
            }

            Contract.Assume(OperationContext.Current != null);

            var callback = OperationContext.Current.GetCallbackChannel<TCallback>();


            if (callback == null)
            {
                throw new ApplicationException("Could not get callback channel");
            }

            Contract.Assume(callback != null);

            return callback;
        }
    }
}