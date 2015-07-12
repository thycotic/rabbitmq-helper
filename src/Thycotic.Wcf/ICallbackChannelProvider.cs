using System.Diagnostics.Contracts;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Interface for a callback channel provider
    /// </summary>
    [ContractClass(typeof(CallbackChannelProviderContract))]
    public interface ICallbackChannelProvider
    {
        /// <summary>
        /// Gets the callback channel.
        /// </summary>
        /// <returns></returns>
        TCallback GetCallbackChannel<TCallback>();
    }

    /// <summary>
    /// Contract for ICallbackChannelProvider
    /// </summary>
    [ContractClassFor(typeof(ICallbackChannelProvider))]
    public abstract class CallbackChannelProviderContract : ICallbackChannelProvider
    {
        /// <summary>
        /// Gets the callback channel.
        /// </summary>
        /// <typeparam name="TCallback"></typeparam>
        /// <returns></returns>
        public TCallback GetCallbackChannel<TCallback>()
        {
            Contract.Ensures(Contract.Result<TCallback>() != null);

            return default(TCallback);
        }
    }
}