using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Thycotic.AppCore;
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

                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.LocalAccounts.Count(),
                    Take = 3
                };
                var truncatedLog = result.Logs.Truncate();
                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
                {
                    var response = new ScanLocalAccountResponse
                    {
                        ComputerId = request.ComputerId,
                        DiscoverySourceId = request.DiscoverySourceId,
                        LocalAccounts = result.LocalAccounts.Skip(paging.Skip).Take(paging.Take).ToArray(),
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
                        _log.Info(string.Format("{0}: Send Local Account Results", request.Input.NameForLog));
                        _responseBus.Execute(response);
                        paging.Skip = paging.NextSkip;
                    }
                    catch (Exception exception)
                    {
                        _log.Info(string.Format("{0}: Send Local Account Results Failed", request.Input.NameForLog),
                            exception);
                    }
                });
            }
            catch (Exception e)
            {
                _log.Info(string.Format("{0} : Scan Local Accounts Failed", request.Input.ComputerName), e);
            }
        }
    }
}
