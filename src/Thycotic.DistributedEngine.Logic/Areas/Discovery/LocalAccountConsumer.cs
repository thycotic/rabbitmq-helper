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
            Contract.Assume(_log != null);
            try
            {
                _log.Info(string.Format("{0} : Scan Local Accounts", request.Input.ComputerName));
                var scanner = this.EnsureNotNull(_scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId), "No scanner returned");
                var result = this.EnsureNotNull(scanner.ScanComputerForLocalAccounts(request.Input), "Scanner returned no result");
                
                _log.Info(string.Format("{0} : Found {1} Local Accounts (Log: {2})",
                    request.Input.ComputerName,
                    result != null ? this.EnsureNotNull(result.LocalAccounts, "Result has no local accounts").Length : -1,
                    result != null ? string.Join("; ", this.EnsureNotNull(result.Logs, "Result has no log items").Select(l => l.Message)) : string.Empty));
                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.LocalAccounts.Count(),
                    Take = request.Input.PageSize
                };
                this.EnsureNotNull(result.Logs, "Logs do not exist");
                var truncatedLog = result.Logs.Truncate();
                

                Enumerable.Range(0, this.EnsureGreaterThanOrEqualTo(paging.BatchCount, 0)).ToList().ForEach(x =>
                {
                    var response = new ScanLocalAccountResponse
                    {
                        BatchId = batchId,
                        ComputerAvailable = result.ComputerAvailable,
                        ComputerId = request.ComputerId,
                        DiscoverySourceId = request.DiscoverySourceId,
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage,
                        LocalAccounts = result.LocalAccounts.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        Logs = truncatedLog,
                        Paging = paging,
                        StatusMessages = { },
                        Success = result.Success
                    };
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
    }
}
