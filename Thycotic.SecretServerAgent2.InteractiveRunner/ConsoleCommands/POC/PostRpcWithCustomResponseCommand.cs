using System;
using System.Linq;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Areas.POC.Response;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands.POC
{
    class PostRpcWithCustomResponseCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(PostRpcWithCustomResponseCommand));

        public override string Name
        {
            get { return "postrpcsort"; }
        }

        public override string Description
        {
            get { return "Posts a sort list rpc message to the exchange"; }
        }

        public PostRpcWithCustomResponseCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                var message = new SortListRpcMessage
                {
                    Items = Enumerable.Range(0, 25).ToList().Select(i => Guid.NewGuid().ToString()).ToArray()
                };

                var response = _bus.RpcPublish<SortListResponse>(message, 30*1000);

                _log.Info("Posting completed.");

                Console.WriteLine("Consumer returned {0} item(s)", response.Items.Length);

                response.Items.ToList().ForEach(Console.WriteLine);
            };
        }
    }
}
