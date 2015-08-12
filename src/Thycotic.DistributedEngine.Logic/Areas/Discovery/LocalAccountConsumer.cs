using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.Discovery.Core.Elements;
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
    /// Local Account Consumer
    /// </summary>
    public class LocalAccountConsumer : IBasicConsumer<ScanLocalAccountMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly IScannerFactory _scannerFactory;
        private readonly ILogWriter _log = Log.Get(typeof(LocalAccountConsumer));

        /// <summary>
        /// Local Account Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="scannerFactory"></param>
        public LocalAccountConsumer(IResponseBus responseBus, IScannerFactory scannerFactory)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(scannerFactory != null);

            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Scan Local Accounts
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanLocalAccountMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Scan Local Accounts", request.Input.ComputerName));
                var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
                var result = scanner.ScanComputerForLocalAccounts(request.Input);
                
                _log.Info(string.Format("{0} : Found {1} Local Accounts (Log: {2})",
                    request.Input.ComputerName,
                    result != null ? result.LocalAccounts.Length : -1,
                    result != null ? string.Join("; ", result.Logs.Select(l => l.Message.Trim())) : string.Empty));

                var paging = new Paging
                {
                    Total = result.LocalAccounts.Count(),
                    Take = request.Input.PageSize
                };

                var truncatedLog = result.Logs.Truncate();
                

                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
                {
                    var response = FormatResponse(request.ComputerId, request.DiscoverySourceId, result.Success,
                        result.ComputerAvailable, result.ErrorMessage,
                        result.LocalAccounts.Skip(paging.Skip).Take(paging.Take).ToArray(), truncatedLog, new string[0],
                        result.ErrorCode, request.BatchId, paging);
                    _log.Info(string.Format("{0}: Send Local Account Results Batch {1} of {2}", request.Input.ComputerName, x + 1, paging.BatchCount));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;
                });
            }
            catch (Exception e)
            {
                _log.Error(string.Format("{0} : Scan Local Accounts Failed", request.Input.ComputerName), e);
            }
        }
        
        private ScanLocalAccountResponse FormatResponse(int computerId, int discoverySourceId, bool success, bool computerAvailable, string errorMessage, 
            LocalAccount[] localAccounts, List<DiscoveryLog> discoveryLogs, string[] statusMessages, int errorCode = 0, Guid batchId = default(Guid), Paging paging = null)
        {
            return new ScanLocalAccountResponse()
            {
                ComputerId = computerId,
                DiscoverySourceId = discoverySourceId,
                Success = success,
                ComputerAvailable = computerAvailable,
                ErrorMessage = errorMessage,
                LocalAccounts = localAccounts,
                Logs = discoveryLogs,
                StatusMessages = statusMessages,
                ErrorCode = errorCode,
                BatchId = batchId,
                Paging = paging
            };
        }
    }
}
