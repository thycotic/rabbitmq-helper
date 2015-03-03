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
        //private readonly ILogWriter _log = Log.Get(typeof(PingConsumer));

        private readonly char[] _characters = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();
        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Consume(PingMessage request)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var index = random.Next(0, _characters.Count() - 1);

            ConsumerConsole.WriteMatrix(_characters[index]);

            ////if (request.Sequence%1000 == 0)
            //{
            //    ConsumerConsole.WriteMatrix('x');
            //}
        }
    }
}
