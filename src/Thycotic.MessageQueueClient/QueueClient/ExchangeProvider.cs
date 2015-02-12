namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Exchange provider
    /// </summary>
    public class ExchangeProvider : IExchangeProvider
    {
        /// <summary>
        /// Gets the current change.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetCurrentChange()
        {
            return "thycotic";
        }
    }
}