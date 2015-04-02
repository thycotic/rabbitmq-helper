using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.Discovery;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Host Range Consumer For Scanning OUs
    /// </summary>
    public class HostRangeConsumer : IBasicConsumer<HostRangeMessage>
    {
        
         private readonly IRequestBus _requestBus;
        private readonly IResponseBus _responseBus;
       
        private readonly IExchangeNameProvider _exchangeNameProvider;


        /// <summary>
        /// Host Range Consumer
        /// </summary>
        /// <param name="exchangeNameProvider">Exchange Name Provider</param>
        /// <param name="requestBus">Request Bus</param>
        /// <param name="responseBus">Response Bus</param>
        public HostRangeConsumer(IExchangeNameProvider exchangeNameProvider, IRequestBus requestBus,IResponseBus responseBus)
        {
            _requestBus = requestBus;
            _responseBus = responseBus;
            _exchangeNameProvider = exchangeNameProvider;
        }


        /// <summary>
        /// Scan the Host Range 
        /// </summary>
        /// <param name="request">Host Range Message to relay to the scanner</param>
        public void Consume(HostRangeMessage request)
        {
          
            

           //var scanner = new HostRangeScanner();


            // var result = SshScanner().ScanHostRange(input);
            // _responseBus.SaveWork(result);

            

            throw new NotImplementedException();
        }
    }
}
