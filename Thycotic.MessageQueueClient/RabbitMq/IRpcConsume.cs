namespace Thycotic.MessageQueueClient.RabbitMq
{
    public interface IRpcConsumer<in TRequest> : IConsumer<TRequest>
    {
    }
}