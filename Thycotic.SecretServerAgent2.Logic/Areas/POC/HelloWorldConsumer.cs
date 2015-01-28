using Thycotic.Logging;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    public class HelloWorldConsumer : IConsumer<HelloWorldMessage>
    {
        private readonly ILogWriter _log = Log.Get(typeof(HelloWorldConsumer));

        public void Consume(HelloWorldMessage request)
        {
            _log.Info(string.Format("CONSUMER: Received message \"{0}\"", request.Content));
            //throw new ApplicationException();
        }
    }
}
