using System;
using System.Diagnostics.Contracts;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a blocking consumer
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    [ContractClass(typeof(BlockingConsumerContract<,>))]
    public interface IBlockingConsumer<in TRequest, out TResponse> : IConsumer
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse Consume(TRequest request);
    }

    /// <summary>
    /// Contract
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    [ContractClassFor(typeof(IBlockingConsumer<,>))]
    public abstract class BlockingConsumerContract<TRequest, TResponse> : IBlockingConsumer<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public TResponse Consume(TRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            return default(TResponse);
        }
    }
}