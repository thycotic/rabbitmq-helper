using System;
using System.Diagnostics.Contracts;
using System.Threading;
using Thycotic.Utility.TestChain;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a consumer that accepts a request
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    [UnitTestsRequired]
    [ContractClass(typeof(BasicConsumerContract<>))]
    public interface IBasicConsumer<in TRequest> : IConsumer 
        where TRequest : class, IBasicConsumable 
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        void Consume(CancellationToken token, TRequest request);
    }

    /// <summary>
    /// Contract 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    [ContractClassFor(typeof(IBasicConsumer<>))]
    public abstract class BasicConsumerContract<TRequest> : IBasicConsumer<TRequest>
        where TRequest : class, IBasicConsumable 
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        public void Consume(CancellationToken token, TRequest request)
        {
            Contract.Requires<ArgumentNullException>(token != null);
            Contract.Requires<ArgumentNullException>(request != null);
        }
    }
}
