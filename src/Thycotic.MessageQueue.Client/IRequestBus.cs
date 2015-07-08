using System;
using System.Diagnostics.Contracts;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client
{
    /// <summary>
    /// Interface for a message bus
    /// </summary>
    [ContractClass(typeof(RequestBusContract))]
    public interface IRequestBus : IDisposable
    {
        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="request">The request.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        void BasicPublish(string exchangeName, IBasicConsumable request, bool persistent = true);

        /// <summary>
        /// Publishes the specified request as remote procedure call. The client will hold until the call succeeds or cails
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="request">The request.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        TResponse BlockingPublish<TResponse>(string exchangeName, IBlockingConsumable request, int timeoutSeconds);
    }

    /// <summary>
    /// Contract for IRequestBus
    /// </summary>
    [ContractClassFor(typeof(IRequestBus))]
    public abstract class RequestBusContract : IRequestBus
    {
        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="request">The request.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        public void BasicPublish(string exchangeName, IBasicConsumable request, bool persistent = true)
        {
            Contract.Requires<ArgumentNullException>(exchangeName != null);
            Contract.Requires<ArgumentNullException>(request != null);
        }

        /// <summary>
        /// Publishes the specified request as remote procedure call. The client will hold until the call succeeds or cails
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="request">The request.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        public TResponse BlockingPublish<TResponse>(string exchangeName, IBlockingConsumable request, int timeoutSeconds)
        {
            Contract.Requires<ArgumentNullException>(exchangeName != null);
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Requires<ArgumentException>(timeoutSeconds > 0);

            return default(TResponse);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}
