using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public interface IRpcConsumer<in TRequest> : IConsumer<TRequest>
    {
    }
}