using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class PostMessageCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(PostMessageCommand));

        public override string Name
        {
            get { return "postmessage"; }
        }

        public override string Description
        {
            get { return "Posts a hello world message to the exchange"; }
        }

        public PostMessageCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to change");

                string content;
                if (!parameters.TryGet("content", out content)) return;

                var message = new HelloWorldMessage
                {
                    Content = content
                };

                _bus.BasicPublish(message);

                _log.Info("Posting completed");

            };
        }
    }
}
