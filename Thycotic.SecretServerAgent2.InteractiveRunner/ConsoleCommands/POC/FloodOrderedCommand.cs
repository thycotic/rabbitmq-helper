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
    class FloodOrderedCommand : ConsoleCommandBase
    {
        private readonly IMessageBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(FloodOrderedCommand));

        public override string Name
        {
            get { return "floodo"; }
        }

        public override string Description
        {
            get { return "Floods the exchange in order"; }
        }

        public FloodOrderedCommand(IMessageBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

                _log.Info(string.Format("Flooding exchange in order with {0} request(s). Please wait...", count));

                for (var loop = 0; loop < count; loop++)
                {
                    var message = new HelloWorldMessage
                    {
                        Content = string.Format("{0} {1}", loop, Guid.NewGuid())
                    };

                    _bus.Publish(message);
                }

                _log.Info("Flooding completed");
            };
        }
    }
}
