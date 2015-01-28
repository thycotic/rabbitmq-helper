using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    public class HelloWorldConsumer : IConsumer<HelloWorldMessage>
    {
        public void Consume(HelloWorldMessage request)
        {
            
        }
    }
}
