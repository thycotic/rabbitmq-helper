namespace Thycotic.MessageQueueClient.RabbitMq
{
    public interface IRpcConsume<in TRequest> : IConsume<TRequest>
    {
    }
}