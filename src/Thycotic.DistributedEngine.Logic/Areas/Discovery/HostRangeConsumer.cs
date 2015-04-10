using System;
using System.Linq;
using Thycotic.AppCore;
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
                    Take = 100,
                    Total = result.HostRangeItems.Count()
                };
                Enumerable.Range(0, paging.PageCount).ToList().ForEach(x =>
                {
                    var response = new ScanHostRangeResponse
                    {
                        DiscoverySourceId = request.DiscoverySourceId,
                        HostRangeItems = result.HostRangeItems.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        Success = result.Success,
                        ErrorCode = result.ErrorCode,
                        StatusMessages = { },
                        Logs = result.Logs,
                        ErrorMessage = result.ErrorMessage,
                        BatchId = batchId,
                        Paging = paging
                    };
                    try
                    {
                        _log.Info(string.Format("{0} : Send Host Range Results", request.Input.Domain));
                        _responseBus.Execute(response);
                        paging.Skip = paging.NextSkip;
                    }
                    catch (Exception exception)
                    {
                        _log.Info(string.Format("{0} : Send Host Range Results Failed", request.Input.Domain), exception);
                    }

                });
            }
            catch (Exception e)
            {
                _log.Info(string.Format("{0} : Scan Host Range Failed using ScannerId: {1}", request.Input.Domain, request.DiscoveryScannerId), e);
            }
        }
    }
}
