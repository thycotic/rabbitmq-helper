using System.Threading;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Slow RPC consumer
    /// </summary>
    public class StepConsumer : IBlockingConsumer<StepMessage, BlockingConsumerResult>, IRegisterForPocOnly
    {
        //private readonly ILogWriter _log = Log.Get(typeof(SlowRpcConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public BlockingConsumerResult Consume(CancellationToken token, StepMessage request)
        {
            ConsumerConsole.WriteLine(string.Format("Received \"{0}\" steps", request.Count));

            var c = request.Count;

            //if statement can be change to gracefully look at token with token.IsCancellationRequested

            while (c > 0)// && !token.IsCancellationRequested)
            {
                //automatic failure.
                token.ThrowIfCancellationRequested();

                ConsumerConsole.Write(". ");
                Thread.Sleep(1000);
                c--;
            }

            return new BlockingConsumerResult {Status = true, StatusText = "Done stepping!"};
        }
    }
}
