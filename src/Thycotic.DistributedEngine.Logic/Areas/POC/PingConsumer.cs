using System;
using System.Linq;
using Thycotic.Logging;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Simple Hello World consumer
    /// </summary>
    public class PingConsumer : IBasicConsumer<PingMessage>
    {
        private readonly IResponseBus _responseBus;

        private readonly ILogWriter _log = Log.Get(typeof(PingConsumer));

        private readonly char[] _characters = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char) i).ToArray();

        /// <summary>
        /// Initializes a new instance of the <see cref="PingConsumer" /> class.
        /// </summary>
        /// <param name="responseBus">The response bus.</param>
        public PingConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }


        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Consume(PingMessage request)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var index = random.Next(0, _characters.Count() - 1);

            ConsumerConsole.WriteMatrix(_characters[index]);

            try
            {
                _responseBus.BasicPublish();
            }
            catch (Exception ex)
            {
                _log.Error("Failed to pong back to server", ex);
            }
        }
    }
}
