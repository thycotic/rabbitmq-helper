using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public static class RoutingHelpers
    {
        public static string GetRoutingKey(this object obj)
        {
            return obj.GetType().FullName;
        }

        //public static string GetQueueName(this ConsumerWrapperBase consumer)
        //{
        //    return consumer.GetType().FullName;
        //}

    }
}
