using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    public class HelloWorldConsumer : IConsumer<HelloWorldMessage>
    {
        private readonly ILogWriter _log = Log.Get(typeof(HelloWorldConsumer));

        public void Consume(HelloWorldMessage request)
        {
            _log.Debug(string.Format("Received message \"{0}\"", request.Content));
        }
    }
}
