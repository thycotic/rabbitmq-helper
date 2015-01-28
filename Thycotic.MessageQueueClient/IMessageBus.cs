using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient
{
    public interface IMessageBus
    {
        TResponse Rpc<TResponse>(IConsumable request, int timeoutSeconds);
        void Publish(IConsumable consumable, bool persistent = true);
    }
}
