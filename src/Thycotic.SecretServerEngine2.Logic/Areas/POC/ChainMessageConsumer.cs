using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerEngine2.Logic.Areas.POC
{
    /// <summary>
    /// Simple Hello World consumer
    /// </summary>
    public class ChainMessageConsumer : IBasicConsumer<ChainMessage>
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
            _bus = bus;
            _exchangeNameProvider = exchangeNameProvider;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Consume(ChainMessage request)
        {
            ConsumerConsole.WriteLine("Received chain message");

            ConsumerConsole.WriteLine("Posting next message...");

            _bus.BasicPublish(_exchangeNameProvider.GetCurrentExchange(), new HelloWorldMessage { Content = "Hello from the chain consumer!"});

            ConsumerConsole.WriteLine("Posted");
        }
    }
}
