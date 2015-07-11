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
            

            try
            {
                _log.Info(string.Format("{0} : Scan Host Range", request.Input.Domain));
                var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
                var result = scanner.ScanForHostRanges(request.Input);
                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.HostRangeItems.Count(),
                    Take = request.Input.PageSize
                };
                var truncatedLog = result.Logs.Truncate();
                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
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
