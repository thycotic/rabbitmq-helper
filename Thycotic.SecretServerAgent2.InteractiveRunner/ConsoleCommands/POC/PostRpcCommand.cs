using System;
using System.Linq;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class PostRpcCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(PostRpcCommand));

        public override string Name
        {
            get { return "postrpc"; }
        }

        public override string Description
        {
            get { return "Posts a rpc message to the exchange"; }
        }

        public PostRpcCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                string stepsString;
                if (!parameters.TryGet("steps", out stepsString)) return;

                var steps = Convert.ToInt32(stepsString);

                var message = new SlowRpcMessage
                {
                    Steps = steps
                };

                var response = _bus.RpcPublish<RpcResult>(message, (steps+5) * 1000);

                _log.Info(string.Format("Posting completed. Consumer said: {0}", response.StatusText));
            };
        }
    }
}
