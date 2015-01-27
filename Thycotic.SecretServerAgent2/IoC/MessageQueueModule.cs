using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Thycotic.Messages;
using Thycotic.SecretServerAgent2.ServiceBus;
using Module = Autofac.Module;

namespace Thycotic.SecretServerAgent2.IoC
{
    class MessageQueueModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<RabbitMqMessageBus>().AsImplementedInterfaces().SingleInstance();

            var messageAssembly = Assembly.GetAssembly(typeof (IConsumable));
            builder.RegisterAssemblyTypes(messageAssembly).Where(t => typeof (IConsumable).IsAssignableFrom(t)).InstancePerDependency();
        }
    }
}
