using System;
using System.Diagnostics.Contracts;
using System.IO;
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
    public class CreateFileHandler :
        //IConsumer<CreateDirectoryMessage>,
        IBlockingConsumer<CreateDirectoryMessage, BlockingConsumerResult>,
        IBasicConsumer<CreateFileMessage>
    {
        private readonly IRequestBus _bus;
        private readonly IExchangeNameProvider _exchangeNameProvider;
        //private readonly ILogWriter _log = Log.Get(typeof(ChainMessage));

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainMessageConsumer" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="exchangeNameProvider">The exchange name provider.</param>
        public CreateFileHandler(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            Contract.Requires<ArgumentNullException>(bus != null);
            Contract.Requires<ArgumentNullException>(exchangeNameProvider != null);
            _bus = bus;
            _exchangeNameProvider = exchangeNameProvider;
        }

        ///// <summary>
        ///// Consumes the specified request.
        ///// </summary>
        ///// <param name="request">The request.</param>
        //public void ConsumeBasic(CreateDirectoryMessage request)
        //{
        //    ConsumerConsole.WriteLine("Received directory message but will wait 2 seconds");

        //    if (!Directory.Exists(request.Path))
        //    {
        //        Thread.Sleep(2 * 1000);
        //        ConsumerConsole.WriteLine(string.Format("Creating directory {0} in fire-and-forget", request.Path));
        //        Directory.CreateDirectory(request.Path);
        //    }

        //}

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public BlockingConsumerResult Consume(CreateDirectoryMessage request)
        {
            ConsumerConsole.WriteLine("Received directory message but will wait 2 seconds");

            if (!Directory.Exists(request.Path))
            {
                Thread.Sleep(2 * 1000);
                ConsumerConsole.WriteLine(string.Format("Creating directory {0} in blocking call", request.Path));
                Directory.CreateDirectory(request.Path);
            }

            return new BlockingConsumerResult { Status = true };
        }


        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Consume(CreateFileMessage request)
        {
            ConsumerConsole.WriteLine("Received file message");

            var path = Path.Combine(Path.GetTempPath(), "SSDEPOC");

            ConsumerConsole.WriteLine("Posting directory message...");
            //_bus.BasicPublish(new CreateDirectoryMessage {FileName = path});
            _bus.BlockingPublish<BlockingConsumerResult>(_exchangeNameProvider.GetCurrentExchange(), new CreateDirectoryMessage { Path = path }, 30);

            ConsumerConsole.WriteLine("Posted directory message...");


            ConsumerConsole.WriteLine(string.Format("Creating file {0}", request.FileName));
            File.CreateText(Path.Combine(path, request.FileName));
            ConsumerConsole.WriteLine("File created");


        }



    }
}
