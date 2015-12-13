using System;
using System.Diagnostics.Contracts;
using System.Threading;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Simple Hello World consumer
    /// </summary>
    public class ChainMessageConsumer : IBasicConsumer<ChainMessage>, IRegisterForPocOnly
    {
        private readonly IRequestBus _bus;
        private readonly IExchangeNameProvider _exchangeNameProvider;
        //private readonly ILogWriter _log = Log.Get(typeof(ChainMessage));

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainMessageConsumer" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="exchangeNameProvider">The exchange name provider.</param>
        public ChainMessageConsumer(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            Contract.Requires<ArgumentNullException>(bus != null);
            Contract.Requires<ArgumentNullException>(exchangeNameProvider != null);
            _bus = bus;
            _exchangeNameProvider = exchangeNameProvider;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        public void Consume(CancellationToken token, ChainMessage request)
        {
            ConsumerConsole.WriteLine("Received chain message");

            ConsumerConsole.WriteLine("Posting next message...");

            _bus.BasicPublish(_exchangeNameProvider.GetCurrentExchange(), new HelloWorldMessage { Content = "Hello from the chain consumer!"});

            ConsumerConsole.WriteLine("Posted");
        }
    }
}
