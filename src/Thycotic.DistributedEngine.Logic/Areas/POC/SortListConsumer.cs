using System.Linq;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Areas.POC.Response;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Sort list RPC consumer
    /// </summary>
    public class SortListConsumer : IBlockingConsumer<SortListMessage, SortListResponse>, IRegisterForPocOnly
    {
        //private readonly ILogWriter _log = Log.Get(typeof(SlowRpcConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SortListResponse Consume(SortListMessage request)
        {
            ConsumerConsole.WriteLine(string.Format("Received \"{0}\" item(s)", request.Items.Length));

            request.Items.ToList().ForEach(i => ConsumerConsole.WriteLine(i));

            return new SortListResponse { Items = request.Items.OrderBy(i => i).ToArray() };
        }
    }
}
