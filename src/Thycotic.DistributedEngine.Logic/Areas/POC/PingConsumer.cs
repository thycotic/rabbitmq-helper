using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.Connectivity.Request;
using Thycotic.Messages.Areas.Connectivity.Response;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Simple Hello World consumer
    /// </summary>
    public class PingConsumer : IBlockingConsumer<PingMessage, PingResponse>
    {
        private readonly IResponseBus _responseBus;

        private readonly ILogWriter _log = Log.Get(typeof(PingConsumer));

        /// <summary>
        /// Initializes a new instance of the <see cref="PingConsumer" /> class.
        /// </summary>
        /// <param name="responseBus">The response bus.</param>
        public PingConsumer(IResponseBus responseBus)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);

            _responseBus = responseBus;
        }


        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public PingResponse Consume(PingMessage request)
        {
            Contract.Ensures(_log != null);

            _log.Debug(string.Format("Consuming ping sequence #{0}", request.Sequence));

            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                _responseBus.ExecuteAsync(new EnginePingRequest());
                stopWatch.Stop();
            }
            catch (Exception ex)
            {
                _log.Error("Failed to pong back to server", ex);
            }

            return new PingResponse
            {
                RoundTripToServerElapsedMilliseconds = stopWatch.ElapsedMilliseconds
            };
        }
    }
}
