using System;
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
            var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
            _log.Info(string.Format("{0}: Scan Local Accounts", request.Input.NameForLog));
            var result = scanner.ScanComputerForLocalAccounts(request.Input);
            var response = new ScanLocalAccountResponse
            {
                ComputerId = request.ComputerId,
                LocalAccounts = result.LocalAccounts,
                Success = result.Success,
                ErrorCode = result.ErrorCode,
                StatusMessages = { },
                Logs = result.Logs,
                ErrorMessage = result.ErrorMessage
            };
            try
            {
                _log.Info(string.Format("{0}: Send Local Account Results", request.Input.NameForLog));
                _responseBus.Execute(response);
            }
            catch (Exception exception)
            {
                _log.Info(string.Format("{0}: Send Local Account Results Failed", request.Input.NameForLog), exception);
            }
        }
    }
}
