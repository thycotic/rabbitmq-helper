using System;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class FloodInSerialCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(FloodInSerialCommand));

        public override string Name
        {
            get { return "floodinserial"; }
        }

        public override string Description
        {
            get { return "Floods the exchange in serial"; }
        }

        public FloodInSerialCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

                _log.Info(string.Format("Flooding exchange in serial with {0} request(s). Please wait...", count));

                for (var loop = 0; loop < count; loop++)
                {
                    var message = new HelloWorldMessage
                    {
                        Content = string.Format("{0} {1}", loop, Guid.NewGuid())
                    };

                    _bus.BasicPublish(message);
                }

                _log.Info("Flooding completed");
            };
        }
    }
}
