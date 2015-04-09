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
    /// Dependency Consumer
    /// </summary>
    public class DependencyConsumer : IBasicConsumer<ScanDependencyMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly IScannerFactory _scannerFactory;
        private readonly ILogWriter _log = Log.Get(typeof(DependencyConsumer));

        /// <summary>
        /// Dependency Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="scannerFactory"></param>
        public DependencyConsumer(IResponseBus responseBus, IScannerFactory scannerFactory)
        {
            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Scan Dependencies
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanDependencyMessage request)
        {
            var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
            _log.Info(string.Format("{0}: Scan Dependencies", request.Input.NameForLog));
            var result = scanner.ScanComputerForDependencies(request.Input);
            var paging = new Paging
            {
                Total = result.DependencyItems.Count()
            };
            Enumerable.Range(0, paging.PageCount).ToList().ForEach(x =>
            {
                var response = new ScanDependencyResponse
                {
                    ComputerId = request.ComputerId,
                    DiscoverySourceId = request.DiscoverySourceId,
                    DependencyItems = result.DependencyItems.Skip(paging.Skip).Take(paging.Take).ToArray(),
                    Success = result.Success,
                    ErrorCode = result.ErrorCode,
                    StatusMessages = { },
                    Logs = result.Logs,
                    ErrorMessage = result.ErrorMessage
                };
                try
                {
                    _log.Info(string.Format("{0}: Send Dependency Results", request.Input.NameForLog));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;
                }
                catch (Exception exception)
                {
                    _log.Info(string.Format("{0}: Send Dependency Results Failed", request.Input.NameForLog), exception);
                }
            });
        }
    }
}
