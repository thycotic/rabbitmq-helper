using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages.Areas.POC;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    public class HelloWorldConsume : IConsume<HelloWorldMessage>
    {
        public void Consume(HelloWorldMessage request)
        {
            
        }
    }
}
