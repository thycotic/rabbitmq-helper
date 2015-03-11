using System;
using Thycotic.ihawu.Business.Federator;
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
        private readonly FederatorProvider _federatorProvider = new FederatorProvider();

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

            var passwordTypeName = request.PasswordInfoProvider.PasswordTypeName;
            try
            {
                var federator = _federatorProvider.GetFederatorByType(passwordTypeName);
                var verifyResult = federator.VerifyCurrentPasswordIsValid(request.PasswordInfoProvider);

                return new SecretHeartbeatResponse
                {
                    Success = verifyResult.Success,
                    ErrorCode = verifyResult.ErrorCode,
                    CommandExecutionResults = verifyResult.CommandExecutionResults,
                    StatusMessages = verifyResult.StatusMessages
                };

            }
            catch (Exception ex)
            {
                _log.Error("Handle specific error here", ex);
                throw;
            }
        }
    }
}
