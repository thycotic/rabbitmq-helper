using System;
using System.IO;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.SecretServerEngine2.InteractiveRunner.ConsoleCommands.POC
{
    class CreateFileCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(CreateFileCommand));

        public override string Name
        {
            get { return "createfile"; }
        }

        public override string Area
        {
            get { return CommandAreas.Poc; }
        }
        
        public override string Description
        {
            get { return "Posts a create temp file message to the exchange"; }
        }

        public CreateFileCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                var message = new CreateFileMessage
                {
                    Path = Path.Combine(Path.GetTempPath(), "SSEPOC", Guid.NewGuid().ToString(), "file.txt")
                };

                _bus.BasicPublish(exchangeNameProvider.GetCurrentExchange(), message);

                _log.Info("Posting completed");

            };
        }
    }
}
