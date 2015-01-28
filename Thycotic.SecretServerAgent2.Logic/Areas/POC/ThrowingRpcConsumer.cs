using System;
using System.Threading;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    public class ThrowingRpcConsumer : IRpcConsumer<ThrowingRpcMessage, RpcResult>
    {
        private readonly ILogWriter _log = Log.Get(typeof(ThrowingRpcConsumer));

        public RpcResult Consume(ThrowingRpcMessage request)
        {
            _log.Error("Throwing!");
            throw new ApplicationException("Throw your hands up in the air!");
        }
    }
}
