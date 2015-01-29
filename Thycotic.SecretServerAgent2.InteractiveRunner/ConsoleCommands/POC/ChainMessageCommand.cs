using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class ChainMessageCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(ChainMessageCommand));

        public override string Name
        {
            get { return "chainmessage"; }
        }

        public override string Description
        {
            get { return "Posts a chain message to the exchange"; }
        }

        public ChainMessageCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to change");

                var message = new ChainMessage();

                _bus.BasicPublish(message);

                _log.Info("Posting completed");

            };
        }
    }
}
