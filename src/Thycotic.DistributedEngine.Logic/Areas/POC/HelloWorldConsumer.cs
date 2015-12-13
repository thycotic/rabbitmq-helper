using System.Threading;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Simple Hello World consumer
    /// </summary>
    public class HelloWorldConsumer : IBasicConsumer<HelloWorldMessage>, IRegisterForPocOnly
    {
        //private readonly ILogWriter _log = Log.Get(typeof(HelloWorldConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        public void Consume(CancellationToken token, HelloWorldMessage request)
        {
            ConsumerConsole.WriteLine(string.Format("Received message \"{0}\"", request.Content));
            //throw new ApplicationException();
        }
    }
}
