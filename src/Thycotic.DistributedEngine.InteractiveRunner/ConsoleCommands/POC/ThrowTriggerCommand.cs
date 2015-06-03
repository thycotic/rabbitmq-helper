using System;
using Thycotic.CLI;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands.POC
{
    class ThrowTriggerCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(ThrowTriggerCommand));

        public override string Name
        {
            get { return "throwtrigger"; }
        }

        public override string Area
        {
            get { return CommandAreas.Poc; }
        }

        public override string Description
        {
            get { return "Posts a throwing blocking message to the exchange"; }
        }

        public ThrowTriggerCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;
            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                var message = new ThrowTriggerMessage();

                try
                {
                    _bus.BlockingPublish<BlockingConsumerResult>(exchangeNameProvider.GetCurrentExchange(), message, 30*1000);

                }
                catch (Exception ex)
                {
                    _log.Error(string.Format("Consumer failed by saying {0}", ex.Message));
                }

                _log.Info("Posting completed.");
            };
        }
    }
}
