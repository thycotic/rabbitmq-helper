using System;
using Thycotic.Logging;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    /// <summary>
    /// Consumer that just throws an exception
    /// </summary>
    public class ThrowingRpcConsumer : IRpcConsumer<ThrowingRpcMessage, RpcResult>
    {
        private readonly ILogWriter _log = Log.Get(typeof(ThrowingRpcConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Throw your hands up in the air!</exception>
        public RpcResult Consume(ThrowingRpcMessage request)
        {
            ConsumerConsole.WriteLine("I got a messages but I will throw!");
            _log.Error("Throwing!");
            throw new ApplicationException("Throw your hands up in the air!");
        }
    }
}
