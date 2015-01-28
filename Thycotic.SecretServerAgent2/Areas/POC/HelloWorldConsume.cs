using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages.Agent;

namespace Thycotic.SecretServerAgent2.Areas.POC
{
    public class HelloWorldConsume : IConsume<HelloWorldMessage>
    {
        public void Consume(HelloWorldMessage request)
        {
            
        }
    }
}
