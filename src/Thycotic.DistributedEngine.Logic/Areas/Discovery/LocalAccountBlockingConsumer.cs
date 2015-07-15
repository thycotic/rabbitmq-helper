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

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Local Account Consumer
    /// </summary>
    public class LocalAccountBlockingConsumer : IBlockingConsumer<ScanLocalAccountBlockingMessage, ScanLocalAccountResponse>
    {
        private readonly IResponseBus _responseBus;
        private readonly IScannerFactory _scannerFactory;
        private readonly ILogWriter _log = Log.Get(typeof(LocalAccountBlockingConsumer));

        /// <summary>
        /// Local Account Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="scannerFactory"></param>
        public LocalAccountBlockingConsumer(IResponseBus responseBus, IScannerFactory scannerFactory)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(scannerFactory != null);

            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ScanLocalAccountResponse Consume(ScanLocalAccountBlockingMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Scan Local Accounts", request.Input.ComputerName));
                var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
                var result = scanner.ScanComputerForLocalAccounts(request.Input);

                _log.Info(string.Format("{0} : Found {1} Local Accounts (Log: {2})",
                    request.Input.ComputerName,
                    result != null ? result.LocalAccounts.Length : -1,
                    result != null ? string.Join("; ", result.Logs.Select(l => l.Message)) : string.Empty));
                var truncatedLog = result.Logs.Truncate();
                var response = new ScanLocalAccountResponse()
                {
                    ComputerId = request.ComputerId,
                    ComputerAvailable = result.ComputerAvailable,
                    DiscoverySourceId = request.DiscoverySourceId,
                    ErrorCode = result.ErrorCode,
                    ErrorMessage = result.ErrorMessage,
                    LocalAccounts = result.LocalAccounts,
                    Logs = truncatedLog,
                    StatusMessages = { },
                    Success = result.Success
                };
                return response;
            }
            catch (Exception e)
            {
                var message = string.Format("{0} : Scan Local Accounts Failed", request.Input.ComputerName);
                _log.Error(message, e);
                var response = new ScanLocalAccountResponse()
                {
                    ComputerId = request.ComputerId,
                    ComputerAvailable = false,
                    DiscoverySourceId = request.DiscoverySourceId,
                    ErrorMessage = message,
                    LocalAccounts = new LocalAccount[0],
                    Logs = new List<DiscoveryLog>(),
                    StatusMessages = { },
                    Success = false
                };
                return response;
            }
        }
    }
}
