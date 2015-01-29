using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient
{
    /// <summary>
    /// Interface for a message bus
    /// </summary>
    public interface IRequestBus
    {
        /// <summary>
        /// Publishes the specified request as an RPC.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        TResponse RpcPublish<TResponse>(IConsumable request, int timeoutSeconds);

        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        void BasicPublish(IConsumable request, bool persistent = true);
    }
}
