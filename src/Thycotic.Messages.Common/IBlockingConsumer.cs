using System;
using System.Diagnostics.Contracts;
using System.Threading;
using Thycotic.Utility.TestChain;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a blocking consumer
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    [UnitTestsRequired]
    [ContractClass(typeof(BlockingConsumerContract<,>))]
    public interface IBlockingConsumer<in TRequest, out TResponse> : IConsumer
        where TRequest : class, IBlockingConsumable
        where TResponse : class
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse Consume(CancellationToken token, TRequest request);
    }

    /// <summary>
    /// Contract
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    [ContractClassFor(typeof(IBlockingConsumer<,>))]
    public abstract class BlockingConsumerContract<TRequest, TResponse> : IBlockingConsumer<TRequest, TResponse>
        where TRequest : class, IBlockingConsumable
        where TResponse : class
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Consume(CancellationToken token, TRequest request)
        {
            Contract.Requires<ArgumentNullException>(token != null);
            Contract.Requires<ArgumentNullException>(request != null);

            Contract.Ensures(Contract.Result<TResponse>() != null);

            return default(TResponse);
        }
    }
}