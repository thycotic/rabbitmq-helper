using System;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class StepCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(StepCommand));

        public override string Name
        {
            get { return "step"; }
        }

        public override string Description
        {
            get { return "Posts a blocking message to the exchange"; }
        }

        public StepCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

                var message = new StepMessage
                {
                    Count = count
                };

                var response = _bus.BlockingPublish<BlockingConsumerResult>(message, (count+5) * 1000);

                _log.Info(string.Format("Posting completed. Consumer said: {0}", response.StatusText));
            };
        }
    }
}
