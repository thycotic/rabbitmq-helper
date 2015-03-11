using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.Heartbeat.Request;
using Thycotic.Messages.Heartbeat.Response;

namespace Thycotic.DistributedEngine.Logic.Areas.Heartbeat
{
    /// <summary>
    /// Secret  heartbeat request
    /// </summary>
    public class SecretHeartbeatConsumer : IBlockingConsumer<SecretHeartbeatMessage, SecretHeartbeatResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(SecretHeartbeatConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SecretHeartbeatResponse Consume(SecretHeartbeatMessage request)
        {

            //request.PasswordInfoProvider

            _log.Info("Got a heartbeat request");

            return new SecretHeartbeatResponse();
        }
    }
}
