namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Exchange provider
    /// </summary>
    public class ExchangeNameProvider : IExchangeNameProvider
    {
        /// <summary>
        /// Gets or sets the name of the exchange.
        /// </summary>
        /// <value>
        /// The name of the exchange.
        /// </value>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Gets the current change.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentExchange()
        {
            return ExchangeName;
        }
    }
}