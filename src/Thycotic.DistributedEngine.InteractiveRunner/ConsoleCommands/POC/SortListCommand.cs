using System;
using System.Linq;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Areas.POC.Response;

namespace Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands.POC
{
    class SortListCommand : CommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(SortListCommand));

        public override string Name
        {
            get { return "sortlist"; }
        }

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

                var response = _bus.BlockingPublish<SortListResponse>(exchangeNameProvider.GetCurrentExchange(), message, 30*1000);

                _log.Info("Posting completed.");

                Console.WriteLine("Consumer returned \"{0}\" item(s)", response.Items.Length);

                response.Items.ToList().ForEach(Console.WriteLine);

                return 0;
            };
        }
    }
}
