using System;
using System.Threading;
using Thycotic.Logging;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Consumer that just throws an exception
    /// </summary>
    public class ThrowTriggerConsumer : IBlockingConsumer<ThrowTriggerMessage, BlockingConsumerResult>, IRegisterForPocOnly
    {
        private readonly ILogWriter _log = Log.Get(typeof(ThrowTriggerConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Throw your hands up in the air!</exception>
        public BlockingConsumerResult Consume(CancellationToken token, ThrowTriggerMessage request)
        {
            ConsumerConsole.WriteLine("I got a messages but I will throw!");
            _log.Error("Throwing!");
            throw new ApplicationException("Throw your hands up in the air!");
        }
    }
}
