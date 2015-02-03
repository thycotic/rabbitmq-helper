using Thycotic.MessageQueueClient.QueueClient;

namespace Thycotic.MessageQueueClient.Wrappers.Proxies
{
    /// <summary>
    /// Common wrapper proxy
    /// </summary>
    public class CommonConsumerWrapperProxy : IConsumerWrapperBase
    {
        /// <summary>
        /// The target of the proxy.
        /// </summary>
        protected readonly IConsumerWrapperBase Target;

        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        public ICommonModel CommonModel
        {
            get { return Target.CommonModel; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonConsumerWrapperProxy"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public CommonConsumerWrapperProxy(IConsumerWrapperBase target)
        {
            Target = target;
        }

        /// <summary>
        /// Starts the consuming.
        /// </summary>
        public void StartConsuming()
        {
            Target.StartConsuming();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Target.Dispose();
        }

    }
}