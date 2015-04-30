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
            return OperationContext.Current.GetCallbackChannel<TCallback>();
        }
    }
}