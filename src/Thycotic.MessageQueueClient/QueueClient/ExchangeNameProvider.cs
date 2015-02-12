namespace Thycotic.MessageQueueClient.QueueClient
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
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetCurrentChange()
        {
            return ExchangeName;
        }
    }
}