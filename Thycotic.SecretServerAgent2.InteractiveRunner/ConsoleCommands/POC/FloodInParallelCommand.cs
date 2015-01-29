using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class FloodInParallelCommand : ConsoleCommand
    {
        private readonly IMessageBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(FloodInParallelCommand));

        public override string Name
        {
            get { return "floodp"; }
        }

        public override string Description
        {
            get { return "Floods the exchange in parallel (or out of order)"; }
        }

        public FloodInParallelCommand(IMessageBus bus)
        {
            _bus = bus;
        }

        public override void Execute(ConsoleCommandParameters parameters)
        {
            string countString;
            if (!parameters.TryGet("count", out countString)) return;

            var count = Convert.ToInt32(countString);

            _log.Info(string.Format("Flooding exchange in parallel with {0} request(s). Please wait...", count));

            Enumerable.Range(0, count).AsParallel().ForAll(i =>
            {
                var message = new HelloWorldMessage
                {
                    Content = string.Format("{0} {1}", i, Guid.NewGuid())
                };

                _bus.Publish(message);
            });

            _log.Info("Flooding completed");
        }
    }
}
