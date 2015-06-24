using System;
using System.Linq;
using System.Threading.Tasks;
using Thycotic.CLI.Commands;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.InteractiveRunner.Commands.POC
{
    class MultiplePublishCommand : CommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(MultiplePublishCommand));

        public override string Area
        {
            get { return CommandAreas.Poc; }
        }

        public override string Description
        {
            get { return "Posts multiple messages to the exchange"; }
        }

        public MultiplePublishCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;

            Action = parameters =>
            {
                var exchangeName = exchangeNameProvider.GetCurrentExchange();

                _log.Info("Posting message to exchange");

                Task.Factory.StartNew(() => _bus.BlockingPublish<BlockingConsumerResult>(exchangeName, new StepMessage { Count = 5 }, 30));

                Enumerable.Range(0,100).ToList().AsParallel().ForAll(i => _bus.BasicPublish(exchangeName, new HelloWorldMessage { Content = Guid.NewGuid().ToString()}));

                _log.Info("Posting completed");

                return 0;

            };
        }
    }
}
