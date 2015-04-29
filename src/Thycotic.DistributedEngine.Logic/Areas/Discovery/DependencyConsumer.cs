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
            try
            {
                _log.Info(string.Format("{0} : Scan Dependencies ({1})", request.Input.ComputerName, GetDependencyTypeName(request.Input.DependencyScannerType)));
                var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
                var result = scanner.ScanComputerForDependencies(request.Input);
                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.DependencyItems.Count(),
                    Take = request.Input.PageSize
                };
                var truncatedLog = result.Logs.Truncate();
                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
                {
                    var response = new ScanDependencyResponse
                    {
                        BatchId = batchId,
                        ComputerId = request.ComputerId,
                        DependencyItems = result.DependencyItems.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        DependencyScannerType = request.Input.DependencyScannerType,
                        DiscoverySourceId = request.DiscoverySourceId,
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage,
                        Logs = truncatedLog,
                        Paging = paging,
                        StatusMessages = { },
                        Success = result.Success
                    };
                    try
                    {
                        _log.Info(string.Format("{0} : Send Dependency ({1}) Results Batch {2} of {3}", request.Input.ComputerName, GetDependencyTypeName(result.DependencyScannerType), x + 1, paging.BatchCount));
                        _responseBus.Execute(response);
                        paging.Skip = paging.NextSkip;
                    }
                    catch (Exception exception)
                    {
                        _log.Error(string.Format("{0} : Send Dependency ({1}) Results Failed", request.Input.ComputerName, GetDependencyTypeName(result.DependencyScannerType)),
                            exception);
                    }
                });
            }
            catch (Exception e)
            {
                _log.Error(string.Format("{0} : Scan Dependencies for DependencyScannerType: {1} Failed using ScannerId: {2}", request.Input.ComputerName, GetDependencyTypeName(request.Input.DependencyScannerType), request.DiscoveryScannerId), e);
            }
        }

        private static string GetDependencyTypeName(int dependencyScannerTypeId)
        {
            switch (dependencyScannerTypeId)
            {
                case 1:
                    return "ApplicationPool";
                case 2:
                    return "WindowsService";
                case 3:
                    return "ScheduledTask";
                default:
                    return string.Format("Unknown ({0})", dependencyScannerTypeId);
            }
        }
    }
}
