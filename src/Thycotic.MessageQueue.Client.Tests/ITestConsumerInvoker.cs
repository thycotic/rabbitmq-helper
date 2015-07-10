using System;
using System.Diagnostics.Contracts;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Tests
{
    /// <summary>
    /// Interface for a handler resolved
    /// </summary>
    [ContractClass(typeof(TestConsumerInvokerContract))]
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

    [ContractClassFor(typeof (ITestConsumerInvoker))]
    public abstract class TestConsumerInvokerContract : ITestConsumerInvoker
    {
        /// <summary>
        /// Consumes the specified consumable.
        /// </summary>
        /// <param name="consumable">The consumable.</param>
        public void Consume(IConsumable consumable)
        {
            Contract.Requires<ArgumentNullException>(consumable != null);
        }

        /// <summary>
        /// Consumes the specified consumable.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="consumable">The consumable.</param>
        /// <returns></returns>
        public TResponse Consume<TResponse>(IConsumable consumable)
        {
            Contract.Requires<ArgumentNullException>(consumable != null);

            Contract.Ensures(Contract.Result<TResponse>() != null);

            return default(TResponse);
        }
    }
}
