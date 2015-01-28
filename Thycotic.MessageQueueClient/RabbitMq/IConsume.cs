namespace Thycotic.MessageQueueClient.RabbitMq
{
    public interface IConsume<in TRequest>
    {
        void Consume(TRequest request);
    }

    public interface IConsume<in TRequest, out TResponse>
    {
        TResponse Consume(TRequest request);
    }

    public interface IRpcConsume<in TRequest, out TResponse> : IConsume<TRequest, TResponse>
    {
    }

}
