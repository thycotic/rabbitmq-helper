namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a consumer
    /// </summary>
    public interface IConsumer
    {
    }


    /// <summary>
    /// Interface for a consumer that accepts a request
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public interface IConsumer<in TRequest> : IConsumer
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Consume(TRequest request);
    }

    //public interface IConsumer<in TRequest, out TResponse> : IConsumer
    //{
    //    TResponse Consume(TRequest request);
    //}

    /// <summary>
    /// Interface for a RPC consumer
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IRpcConsumer<in TRequest, out TResponse> : IConsumer
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse Consume(TRequest request);
    }

}
