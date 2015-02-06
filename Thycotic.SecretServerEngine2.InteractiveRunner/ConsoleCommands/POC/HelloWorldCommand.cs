using System;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.SecretServerEngine2.InteractiveRunner.ConsoleCommands.POC
{
    class HelloWorldCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(HelloWorldCommand));

        public override string Name
        {
            get { return "helloworld"; }
        }

        public override string Area {
            get { return CommandAreas.Poc; }
        }

        public override string Description
        {
            get { return "Posts a hello world message to the exchange"; }
        }

        public HelloWorldCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

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
