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
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(scannerFactory != null);

            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Scan Machines
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanMachineMessage request)
        {
            Contract.Assume(_log != null);

            try
            {
                _log.Info(string.Format("{0} : Scan Machines", request.Input.HostRange));
                var scanner = this.EnsureNotNull(_scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId),"No scanner returned");
                
                var result = this.EnsureNotNull(scanner.ScanForMachines(request.Input),"No result returned");

                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = this.EnsureNotNull(result.Computers,"No computers were returned from result.").Count(),
                    Take = request.Input.PageSize
                };

                var truncatedLog = this.EnsureNotNull(result.Logs,"No logs were returned from result").Truncate();
                
                Enumerable.Range(0, this.EnsureGreaterThanOrEqualTo(paging.BatchCount,0)).ToList().ForEach(x =>
                {
                    var response = new ScanMachineResponse
                    {
                        BatchId = batchId,
                        ComputerItems = result.Computers.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        DiscoverySourceId = request.DiscoverySourceId,
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage,
                        HostRangeName = result.RangeName,
                        Logs = truncatedLog,
                        Paging = paging,
                        StatusMessages = { },
                        Success = result.Success,
                        SpecificOu = request.Input.SpecificOu
                    };
                    _log.Info(string.Format("{0} : Send Machine Results Batch {1} of {2}", request.Input.HostRange, x + 1, paging.BatchCount));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;                        
                });
            }
            catch (Exception e)
            {
                _log.Error(string.Format("{0} : Scan Machines Failed using ScannerId: {1}", request.Input.HostRange, request.DiscoveryScannerId), e);
            }
        }
    }
}
