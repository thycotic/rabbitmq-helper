using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    /// <summary>
    /// Interface for an RPC consumer
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public interface IRpcConsumer<in TRequest> : IConsumer<TRequest>
    {
    }
}