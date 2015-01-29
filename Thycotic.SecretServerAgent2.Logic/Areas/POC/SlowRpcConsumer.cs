using System;
using System.Threading;
using Thycotic.Logging;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    /// <summary>
    /// Slow RPC consumer
    /// </summary>
    public class SlowRpcConsumer : IRpcConsumer<SlowRpcMessage, RpcResult>
    {
        private readonly ILogWriter _log = Log.Get(typeof(SlowRpcConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public RpcResult Consume(SlowRpcMessage request)
        {
            ConsumerConsole.WriteLine(string.Format("Received \"{0}\" items", request.Items.Length));

            //do something silly here
            var c = 5;

            while (c > 0)
            {
                ConsumerConsole.Write(".");
                Thread.Sleep(1000);
                c--;
            }

            return new RpcResult {Status = true, StatusText = "Wow that took a while"};
        }
    }
}
