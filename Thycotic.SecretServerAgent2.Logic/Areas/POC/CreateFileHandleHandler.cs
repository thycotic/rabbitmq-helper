using System;
using System.IO;
using System.Threading;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    /// <summary>
    /// Simple Hello World consumer
    /// </summary>
    public class CreateFileHandler :
        IConsumer<CreateDirectoryMessage>,
        //IRpcConsumer<CreateDirectoryMessage, RpcResult>,
        IConsumer<CreateFileMessage>
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(ChainMessage));

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainMessageConsumer"/> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        public CreateFileHandler(IRequestBus bus)
        {
            _bus = bus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Consume(CreateDirectoryMessage request)
        {
            if (!Directory.Exists(request.Path))
            {
                Thread.Sleep(1*1000);
                ConsumerConsole.WriteLine(string.Format("Creating {0} in fire-and-forget", request.Path));
                Directory.CreateDirectory(request.Path);
            }

        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public RpcResult ConsumeRpc(CreateDirectoryMessage request)
        {
            if (!Directory.Exists(request.Path))
            {
                Thread.Sleep(1 * 1000);
                ConsumerConsole.WriteLine(string.Format("Creating {0} in RPC", request.Path));
                Directory.CreateDirectory(request.Path);
            }

            return new RpcResult { Status = true };
        }


        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Consume(CreateFileMessage request)
        {
            ConsumerConsole.WriteLine("Received file message");

            ConsumerConsole.WriteLine("Posting directory message...");

            var path = Path.GetDirectoryName(request.Path);

            _bus.BasicPublish(new CreateDirectoryMessage {Path = path});
            //_bus.RpcPublish<RpcResult>(new CreateDirectoryMessage { Path = path }, 30);

            ConsumerConsole.WriteLine(string.Format("Creating {0}", request.Path));

            File.CreateText(request.Path);
            
            
        }



    }
}
