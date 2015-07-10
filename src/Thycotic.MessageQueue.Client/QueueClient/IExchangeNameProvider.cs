using System.Diagnostics.Contracts;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Interface for an exchange provider
    /// </summary>
    [ContractClass(typeof(ExchangeNameProviderContract))]
    public interface IExchangeNameProvider
    {
        /// <summary>
        /// Gets the current change.
        /// </summary>
        /// <returns></returns>
        string GetCurrentExchange();

    }

    /// <summary>
    /// Contract for IExchangeNameProvider
    /// </summary>
    [ContractClassFor(typeof(IExchangeNameProvider))]
    public abstract class ExchangeNameProviderContract : IExchangeNameProvider
    {
        /// <summary>
        /// Gets the current change.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentExchange()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            return default(string);
        }
    }
}