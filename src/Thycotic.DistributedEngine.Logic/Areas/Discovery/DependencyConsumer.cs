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
    /// Dependency Consumer
    /// </summary>
    public class DependencyConsumer : IBasicConsumer<ScanDependencyMessage>
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
        /// Dependency Consumer
        /// </summary>
        /// <param name="exchangeNameProvider"></param>
        /// <param name="requestBus"></param>
        /// <param name="responseBus"></param>
        public DependencyConsumer(IExchangeNameProvider exchangeNameProvider, IRequestBus requestBus, IResponseBus responseBus)
        {
            _requestBus = requestBus;
            _responseBus = responseBus;
            _exchangeNameProvider = exchangeNameProvider;
        }

        /// <summary>
        /// Scan Dependencies
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanDependencyMessage request)
        {
            // do the scanning
            var scanner = ScannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
            var result = scanner.ScanComputerForDependencies(request.Input);
            var response = new ScanDependencyResponse
            {
                DependencyItems = result.DependencyItems,
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
