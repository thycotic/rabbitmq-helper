namespace Thycotic.MessageQueueClient
{
    public interface IConsumer<in TRequest>
    {
        void Consume(TRequest request);
    }

    public interface IConsumer<in TRequest, out TResponse>
    {
        TResponse Consume(TRequest request);
    }

    public interface IRpcConsumer<in TRequest, out TResponse> : IConsumer<TRequest, TResponse>
    {

    }

}
