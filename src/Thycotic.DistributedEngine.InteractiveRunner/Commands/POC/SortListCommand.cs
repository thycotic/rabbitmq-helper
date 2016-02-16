using System;
using System.Linq;
using Thycotic.CLI.Commands;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.DE.Areas.POC.Request;
using Thycotic.Messages.DE.Areas.POC.Response;

namespace Thycotic.DistributedEngine.InteractiveRunner.Commands.POC
{
    class SortListCommand : CommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(SortListCommand));

        public override string Area
        {
            get { return CommandAreas.Poc; }
        }

        public override string Description
        {
            get { return "Posts a sort list blocking message to the exchange"; }
        }

        public SortListCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                var message = new SortListMessage
                {
                    Items = Enumerable.Range(0, 25).ToList().Select(i => Guid.NewGuid().ToString()).ToArray()
                };

                var response = _bus.BlockingPublish<SortListResponse>(exchangeNameProvider.GetCurrentExchange(), message, 30);

                _log.Info("Posting completed.");

                Console.WriteLine("Consumer returned \"{0}\" item(s)", response.Items.Length);

                response.Items.ToList().ForEach(Console.WriteLine);

                return 0;
            };
        }
    }
}
