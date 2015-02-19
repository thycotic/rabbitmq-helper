using System.Threading;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerEngine.Logic.Areas.POC
{
    /// <summary>
    /// Slow RPC consumer
    /// </summary>
    public class StepConsumer : IBlockingConsumer<StepMessage, BlockingConsumerResult>
    {
        //private readonly ILogWriter _log = Log.Get(typeof(SlowRpcConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public BlockingConsumerResult Consume(StepMessage request)
        {
            ConsumerConsole.WriteLine(string.Format("Received \"{0}\" steps", request.Count));

            var c = request.Count;

            while (c > 0)
            {
                ConsumerConsole.Write(". ");
                Thread.Sleep(1000);
                c--;
            }

            return new BlockingConsumerResult {Status = true, StatusText = "Done stepping!"};
        }
    }
}
