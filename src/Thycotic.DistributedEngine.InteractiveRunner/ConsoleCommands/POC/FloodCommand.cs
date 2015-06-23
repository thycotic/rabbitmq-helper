using System;
using System.Threading;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands.POC
{
    class FloodCommand : CommandBase
    {
        private readonly IRequestBus _bus;
        private readonly CancellationToken _cancellationToken;
        private readonly ILogWriter _log = Log.Get(typeof(FloodCommand));

        public override string Name
        {
            get { return "flood"; }
        }

        public override string Area
        {
            get { return CommandAreas.Poc; }
        }

        public override string Description
        {
            get { return "Floods the exchange with messages"; }
        }

        public FloodCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider, CancellationToken cancellationToken)
        {
            _bus = bus;
            _cancellationToken = cancellationToken;

            Action = parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return 1;

                var count = Math.Max(0, Convert.ToInt32(countString));

                _log.Info(string.Format("Flooding exchange with {0} request(s). Please wait...", count));
                
                try
                {
                    var i = 0;
                    while (i++ < count && !_cancellationToken.IsCancellationRequested)
                    {
                        var message = new PingMessage
                        {
                            Sequence = i
                        };

                        _bus.BasicPublish(exchangeNameProvider.GetCurrentExchange(), message);

                        if (i%10000 == 0)
                        {
                            _log.Info(string.Format("Flooded {0} requests", i));
                        }
                    }

                    _log.Info("Flooding completed");

                    return 0;
                }
                catch (ObjectDisposedException)
                {
                    _log.Info("Flooding aborted");

                    return 0;
                }

            };
        }
    }
}
