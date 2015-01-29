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
        private readonly IMessageBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(PostRpcCommand));

        public override string Name
        {
            get { return "postrpc"; }
        }

        public override string Description
        {
            get { return "Posts a rpc message to the exchange"; }
        }

        public PostRpcCommand(IMessageBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                var message = new SlowRpcMessage
                {
                    Items = Enumerable.Range(0, 1).ToList().Select(i => Guid.NewGuid().ToString()).ToArray()
                };

                var response = _bus.Rpc<RpcResult>(message, 30*1000);

                _log.Info(string.Format("Posting completed. Consumer said: {0}", response.StatusText));
            };
        }
    }
}
