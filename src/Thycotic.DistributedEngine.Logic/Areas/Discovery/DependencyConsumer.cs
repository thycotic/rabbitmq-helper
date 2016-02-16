using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Thycotic.Discovery.Core.Elements;
using Thycotic.Discovery.Core.Results;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.DE.Areas.Discovery.Request;
using Thycotic.Messages.Common;
using Thycotic.SharedTypes.General;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Dependency Consumer
    /// </summary>
    public class DependencyConsumer : IBasicConsumer<ScanDependencyMessage>, IBlockingConsumer<ScanDependencyBlockingMessage, ScanDependencyResponse>
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
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(scannerFactory != null);

            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Scan Dependencies
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        public void Consume(CancellationToken token, ScanDependencyMessage request)
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
                    var response = FormatResponse(request.ComputerId, request.DiscoverySourceId, result.ErrorMessage,
                        truncatedLog, new string[0], result.Success,
                        result.DependencyItems.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        request.Input.DependencyScannerType, result.ErrorCode, batchId, paging);
                    _log.Info(string.Format("{0} : Send Dependency ({1}) Results Batch {2} of {3}", request.Input.ComputerName, GetDependencyTypeName(result.DependencyScannerType), x + 1, paging.BatchCount));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;
                });
            }
            catch (Exception e)
            {
                _log.Error(string.Format("{0} : Scan Dependencies for DependencyScannerType: {1} Failed using ScannerId: {2}", request.Input.ComputerName, GetDependencyTypeName(request.Input.DependencyScannerType), request.DiscoveryScannerId), e);
            }
        }

        /// <summary>
        /// Scan Dependencies
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ScanDependencyResponse Consume(CancellationToken token, ScanDependencyBlockingMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Scan Dependencies ({1})", request.Input.ComputerName, GetDependencyTypeName(request.Input.DependencyScannerType)));
                var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
                var result = scanner.ScanComputerForDependencies(request.Input);
                var truncatedLog = result.Logs.Truncate();
                _log.Info(string.Format("{0} : Send Dependency ({1})", request.Input.ComputerName, GetDependencyTypeName(result.DependencyScannerType)));
                return FormatResponse(request.ComputerId, request.DiscoverySourceId, result.ErrorMessage,
                    truncatedLog, new string[0], result.Success, result.DependencyItems,
                    request.Input.DependencyScannerType, result.ErrorCode);
            }
            catch (Exception e)
            {
                var message = string.Format("{0} : Scan Dependencies for DependencyScannerType: {1} Failed using ScannerId: {2}", request.Input.ComputerName, GetDependencyTypeName(request.Input.DependencyScannerType), request.DiscoveryScannerId);
                _log.Error(message, e);
                return FormatResponse(request.ComputerId, request.DiscoverySourceId, message,
                    new List<DiscoveryLog>(), new string[0], false, new DependencyItem[0],
                    request.Input.DependencyScannerType);
            }
        }

        private ScanDependencyResponse FormatResponse(int computerId, int discoverySourceId, string errorMessage, List<DiscoveryLog> logs, 
            string[] statusMessages, bool success, DependencyItem[] dependencyItems, int dependencyScannerType, int errorCode = 0, Guid batchId = default(Guid), Paging paging = null)
        {
            return new ScanDependencyResponse
            {
                ComputerId = computerId,
                DiscoverySourceId = discoverySourceId,
                ErrorMessage = errorMessage,
                Logs = logs,
                StatusMessages = statusMessages,
                Success = success,
                DependencyItems = dependencyItems,
                DependencyScannerType = dependencyScannerType,
                ErrorCode = errorCode,
                BatchId = batchId,
                Paging = paging
            };
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
