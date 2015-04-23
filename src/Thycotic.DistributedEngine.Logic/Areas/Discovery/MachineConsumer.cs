using System;
using System.Linq;
using Thycotic.Discovery.Core.Results;
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
    /// Machine Consumer
    /// </summary>
    public class MachineConsumer : IBasicConsumer<ScanMachineMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly IScannerFactory _scannerFactory;
        private readonly ILogWriter _log = Log.Get(typeof(MachineConsumer));

        /// <summary>
        /// Machine Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="scannerFactory"></param>
        public MachineConsumer(IResponseBus responseBus, IScannerFactory scannerFactory)
        {
            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Scan Machines
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanMachineMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Scan Machines", request.Input.HostRange));
                var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
                var result = scanner.ScanForMachines(request.Input);
                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.Computers.Count(),
                    Take = request.Input.PageSize
                };
                var truncatedLog = result.Logs.Truncate();
                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
                {
                    var response = new ScanMachineResponse
                    {
                        ComputerItems = result.Computers.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        DiscoverySourceId = request.DiscoverySourceId,
                        HostRangeName = result.RangeName,
                        Success = result.Success,
                        ErrorCode = result.ErrorCode,
                        StatusMessages = { },
                        Logs = truncatedLog,
                        ErrorMessage = result.ErrorMessage,
                        BatchId = batchId,
                        Paging = paging
                    };
                    try
                    {
                        _log.Info(string.Format("{0} : Send Machine Results Batch {1} of {2}", request.Input.HostRange, x + 1, paging.BatchCount));
                        _responseBus.Execute(response);
                        paging.Skip = paging.NextSkip;
                    }
                    catch (Exception exception)
                    {
                        _log.Info(string.Format("{0} : Send Machine Results Failed", request.Input.HostRange), exception);
                    }
                });
            }
            catch (Exception e)
            {
                _log.Info(string.Format("{0} : Scan Machines Failed using ScannerId: {1}", request.Input.HostRange, request.DiscoveryScannerId), e);
            }
        }
    }
}
