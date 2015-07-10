using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.Discovery.Request;
using Thycotic.Messages.Common;
using Thycotic.SharedTypes.General;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Host Range Consumer
    /// </summary>
    public class HostRangeConsumer : IBasicConsumer<ScanHostRangeMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly IScannerFactory _scannerFactory;
        private readonly ILogWriter _log = Log.Get(typeof(HostRangeConsumer));

        /// <summary>
        /// Host Range Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="scannerFactory"></param>
        public HostRangeConsumer(IResponseBus responseBus, IScannerFactory scannerFactory)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(scannerFactory!= null);

            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Scan Host Range
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanHostRangeMessage request)
        {
            Contract.Assume(_log != null);

            try
            {
                _log.Info(string.Format("{0} : Scan Host Range", request.Input.Domain));
                var scanner = this.EnsureNotNull(_scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId), "No scanner returned");
                var result = this.EnsureNotNull(scanner.ScanForHostRanges(request.Input), "Scanner returned no result");
                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = this.EnsureNotNull(result.HostRangeItems, "Result has not host ranges").Count(),
                    Take = request.Input.PageSize
                };
                var truncatedLog = this.EnsureNotNull(result.Logs, "Result has no log items").Truncate();
                Enumerable.Range(0, this.EnsureGreaterThanOrEqualTo(paging.BatchCount, 0)).ToList().ForEach(x =>
                {
                    var response = new ScanHostRangeResponse
                    {
                        BatchId = batchId,
                        DiscoverySourceId = request.DiscoverySourceId,
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage,
                        HostRangeItems = result.HostRangeItems.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        Logs = truncatedLog,
                        Paging = paging,
                        StatusMessages = { },
                        Success = result.Success
                    };
                    _log.Info(string.Format("{0} : Send Host Range Results Batch {1} of {2}", request.Input.Domain, x + 1, paging.BatchCount));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;
                });
            }
            catch (Exception e)
            {
                _log.Error(string.Format("{0} : Scan Host Range Failed using ScannerId: {1}", request.Input.Domain, request.DiscoveryScannerId), e);
            }
        }
    }
}
