using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.Areas.Discovery.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Dependency Consumer
    /// </summary>
    public class DependencyConsumer : IBasicConsumer<ScanDependencyMessage>
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
        /// Dependency Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        public DependencyConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }

        /// <summary>
        /// Scan Dependencies
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanDependencyMessage request)
        {
            var scanner = ScannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
            var result = scanner.ScanComputerForDependencies(request.Input);
            var response = new ScanDependencyResponse
            {
                ComputerId = request.ComputerId,
                DependencyItems = result.DependencyItems,
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
