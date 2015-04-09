using System;
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
            var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
            _log.Info(string.Format("{0}: Scan Machines", request.Input.NameForLog));
            var result = scanner.ScanForMachines(request.Input);
            var batchId = Guid.NewGuid();
            var paging = new Paging
            {
                Total = result.Computers.Count()
            };
            Enumerable.Range(0, paging.PageCount).ToList().ForEach(x =>
            {
                var response = new ScanMachineResponse
                {
                    ComputerItems = result.Computers.Skip(paging.Skip).Take(paging.Take).ToArray(),
                    DiscoverySourceId = request.DiscoverySourceId,
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
                    _log.Info(string.Format("{0}: Send Machine Results", request.Input.NameForLog));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;
                }
                catch (Exception exception)
                {
                    _log.Info(string.Format("{0}: Send Machine Results Failed", request.Input.NameForLog), exception);
                }
            });
        }
    }
}
