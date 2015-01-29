using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class PostRpcThrowingCommand : ConsoleCommandBase
    {
        private readonly IMessageBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(PostRpcThrowingCommand));

        public override string Name
        {
            get { return "postrpct"; }
        }

        public override string Description
        {
            get { return "Posts a throwing rpc message to the exchange"; }
        }

        public PostRpcThrowingCommand(IMessageBus bus)
        {
            _bus = bus;
            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                var message = new ThrowingRpcMessage();

                try
                {
                    _bus.Rpc<RpcResult>(message, 30*1000);

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
