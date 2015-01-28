using System;
using System.Threading;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    public class SlowRpcConsumer : IRpcConsumer<SlowRpcMessage, RpcResult>
    {
        private readonly ILogWriter _log = Log.Get(typeof(SlowRpcConsumer));

        public RpcResult Consume(SlowRpcMessage request)
        {
            _log.Debug(string.Format("CONSUMER: Received \"{0}\" items", request.Items.Length));
            //throw new ApplicationException();

            //do something silly here
            var c = 5;

            while (c > 0)
            {
                Console.Write(".");
                Thread.Sleep(1000);
                c--;
            }

            return new RpcResult {Status = true, StatusText = "Wow that took a while"};
        }
    }
}
