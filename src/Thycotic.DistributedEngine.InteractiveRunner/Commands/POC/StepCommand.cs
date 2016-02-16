using System;
using Thycotic.CLI.Commands;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.DE.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.InteractiveRunner.Commands.POC
{
    class StepCommand : CommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(StepCommand));

        public override string Area
        {
            get { return CommandAreas.Poc; }
        }

        public override string Description
        {
            get { return "Posts a blocking message to the exchange"; }
        }

        public StepCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                string countString;
                if (!parameters.TryGet("count", out countString)) return 1;

                var count = Convert.ToInt32(countString);

                var message = new StepMessage
                {
                    Count = count
                };

                var response = _bus.BlockingPublish<BlockingConsumerResult>(exchangeNameProvider.GetCurrentExchange(), message, (count+5) * 1000);

                _log.Info(string.Format("Posting completed. Consumer said: {0}", response.StatusText));

                return 0;
            };
        }
    }
}
