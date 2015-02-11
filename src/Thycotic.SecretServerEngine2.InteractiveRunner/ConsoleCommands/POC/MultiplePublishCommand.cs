using System;
using System.Linq;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerEngine2.InteractiveRunner.ConsoleCommands.POC
{
    class MultiplePublishCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(MultiplePublishCommand));

        public override string Name
        {
            get { return "multiplepublish"; }
        }

        public override string Area
        {
            get { return CommandAreas.Poc; }
        }

        public override string Description
        {
            get { return "Posts multiple messages to the exchange"; }
        }

        public MultiplePublishCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                Task.Factory.StartNew(() => _bus.BlockingPublish<BlockingConsumerResult>(new StepMessage { Count = 5 }, 30));

                Enumerable.Range(0,100).ToList().AsParallel().ForAll(i => _bus.BasicPublish(new HelloWorldMessage { Content = Guid.NewGuid().ToString()}));

                _log.Info("Posting completed");

            };
        }
    }
}
