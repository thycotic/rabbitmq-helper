namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a consumer that accepts a request
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public interface IBasicConsumer<in TRequest> : IConsumer
    {
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Consume(TRequest request);
    }
}
