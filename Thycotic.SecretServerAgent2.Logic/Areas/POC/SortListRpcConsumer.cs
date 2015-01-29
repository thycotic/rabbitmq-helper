using System;
using System.Linq;
using Thycotic.Logging;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Areas.POC.Response;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    /// <summary>
    /// Sort list RPC consumer
    /// </summary>
    public class SortListRpcConsumer : IRpcConsumer<SortListRpcMessage, SortListResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(SlowRpcConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SortListResponse Consume(SortListRpcMessage request)
        {
            _log.Info(string.Format("CONSUMER: Received \"{0}\" items", request.Items.Length));

            request.Items.ToList().ForEach(Console.WriteLine);

            return new SortListResponse { Items = request.Items.OrderBy(i => i).ToArray() };
        }
    }
}
