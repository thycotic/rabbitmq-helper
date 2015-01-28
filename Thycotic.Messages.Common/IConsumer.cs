namespace Thycotic.Messages.Common
{
    public interface IConsumer
    {
    }

    public interface IConsumer<in TRequest> : IConsumer
    {
        void Consume(TRequest request);
    }

    //public interface IConsumer<in TRequest, out TResponse> : IConsumer
    //{
    //    TResponse Consume(TRequest request);
    //}

    public interface IRpcConsumer<in TRequest, out TResponse> : IConsumer
    {
        TResponse Consume(TRequest request);
    }

}
