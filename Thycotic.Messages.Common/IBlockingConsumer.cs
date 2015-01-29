namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a RPC consumer
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IBlockingConsumer<in TRequest, out TResponse> : IConsumer
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse Consume(TRequest request);
    }
}