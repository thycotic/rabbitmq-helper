namespace Thycotic.MessageQueueClient
{
    public interface IMessageBus
    {
        TResponse Rpc<TResponse>(object request, int timeoutSeconds);
        void Publish(object consumable, bool persistent = true);
    }
}
