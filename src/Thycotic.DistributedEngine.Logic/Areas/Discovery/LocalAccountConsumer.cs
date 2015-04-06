using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
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

        /// <summary>
        /// Version
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Retry Count
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Local Account Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        public LocalAccountConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }

        /// <summary>
        /// Scan Local Accounts
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanLocalAccountMessage request)
        {
            var scanner = ScannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
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
            _responseBus.Execute(response);
        }
    }
}
