using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Tests
{
    /// <summary>
    /// Interface for a handler resolved
    /// </summary>
    public interface ITestConsumerInvoker
    {
        /// <summary>
        /// Consumes the specified consumable.
        /// </summary>
        /// <param name="consumable">The consumable.</param>
        void Consume(IConsumable consumable);

        /// <summary>
        /// Consumes the specified consumable.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="consumable">The consumable.</param>
        /// <returns></returns>
        TResponse Consume<TResponse>(IConsumable consumable);

    }
}
