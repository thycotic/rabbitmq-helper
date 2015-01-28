using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
