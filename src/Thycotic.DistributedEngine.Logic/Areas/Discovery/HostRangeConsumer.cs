using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.Discovery.Core.Inputs;
using Thycotic.Discovery.Core.Results;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.Discovery;
using Thycotic.Messages.Common;
using Thycotic.Messages.PasswordChanging.Request;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// 
    /// </summary>
    public class HostRangeConsumer : IBasicConsumer<ScanHostRangeMessage>
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
        /// Host Range Consumer
        /// </summary>
        /// <param name="exchangeNameProvider"></param>
        /// <param name="requestBus"></param>
        /// <param name="responseBus"></param>
        public HostRangeConsumer(IExchangeNameProvider exchangeNameProvider, IRequestBus requestBus, IResponseBus responseBus)
        {
            _requestBus = requestBus;
            _responseBus = responseBus;
            _exchangeNameProvider = exchangeNameProvider;
        }

        /// <summary>
        /// Scan Host Range
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanHostRangeMessage request)
        {
            // do the scanning

            var scanner = ScannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
            var result = scanner.ScanForHostRanges(request.ScanHostRangeInput);



            // call back to server

            // queue up the next process
        }
    }
}
