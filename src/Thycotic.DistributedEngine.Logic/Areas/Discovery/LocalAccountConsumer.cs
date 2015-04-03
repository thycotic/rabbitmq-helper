using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.Discovery.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Local Account Consumer
    /// </summary>
    public class LocalAccountConsumer : IBasicConsumer<ScanLocalAccountMessage>
    {
        private readonly IRequestBus _requestBus;
        private readonly IResponseBus _responseBus;
        private readonly IExchangeNameProvider _exchangeNameProvider;



        /// <summary>
        /// Version
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Retry Count
        /// </summary>
        public int RetryCount { get; set; }


        //private readonly ILogWriter _log = Log.Get(typeof(ChainMessage));

        /// <summary>
        /// Local Account Consumer
        /// </summary>
        /// <param name="exchangeNameProvider"></param>
        /// <param name="requestBus"></param>
        /// <param name="responseBus"></param>
        public LocalAccountConsumer(IExchangeNameProvider exchangeNameProvider, IRequestBus requestBus, IResponseBus responseBus)
        {
            _requestBus = requestBus;
            _responseBus = responseBus;
            _exchangeNameProvider = exchangeNameProvider;
        }

        /// <summary>
        /// Scan Local Accounts
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanLocalAccountMessage request)
        {
            // do the scanning

            var scanner = ScannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
            var result = scanner.ScanComputerForLocalAccounts(request.Input);
            var response = new ScanLocalAccountResponse
            {
                LocalAccounts = result.LocalAccounts,
                Success = result.Success,
                ErrorCode = result.ErrorCode,
                StatusMessages = { },
                Logs = result.Logs,
                ErrorMessage = result.ErrorMessage
            };

            // call back to server
            _responseBus.Execute(response);
        }
    }
}
