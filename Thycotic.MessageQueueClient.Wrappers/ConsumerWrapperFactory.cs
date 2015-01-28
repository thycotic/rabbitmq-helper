using Autofac;

namespace Thycotic.MessageQueueClient.Wrappers
{
    public class ConsumerWrapperFactory : IConsumerWrapperFactory, IStartable
    {
        private readonly IContainer _container;

        public ConsumerWrapperFactory(IContainer container)
        {
            _container = container;
        }

        public void Start()
        {
            var temp = _container.ComponentRegistry.Registrations;
        }
    }
}
