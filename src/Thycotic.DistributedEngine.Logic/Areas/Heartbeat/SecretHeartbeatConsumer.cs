using System;
using System.Linq;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.General;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.ihawu.Business.Federator;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.Heartbeat.Request;

namespace Thycotic.DistributedEngine.Logic.Areas.Heartbeat
{
    /// <summary>
    /// Secret  heartbeat request
    /// </summary>
    public class SecretHeartbeatConsumer : IBasicConsumer<SecretHeartbeatMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly FederatorProvider _federatorProvider = new FederatorProvider();

        private readonly ILogWriter _log = Log.Get(typeof(SecretHeartbeatConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        public SecretHeartbeatConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void Consume(SecretHeartbeatMessage request)
        {
            _log.Info(string.Format("Got a heartbeat request for Secret Id {0}:", request.SecretId));

            var passwordTypeName = request.PasswordInfoProvider.PasswordTypeName;
            try
            {
                var federator = _federatorProvider.GetFederatorByType(passwordTypeName);
                var verifyResult = federator.VerifyCurrentPasswordIsValid(request.PasswordInfoProvider);

                var response = new SecretHeartbeatResponse
                {
                    Success = verifyResult.Success,
                    SecretId = request.SecretId,
                    ErrorCode = (int)verifyResult.ErrorCode,
                    CommandExecutionResults = (verifyResult.CommandExecutionResults ?? new AppCore.Federator.CommandExecutionResult[0]).Select(cer => new CommandExecutionResult
                    {
                        Command = cer.Command,
                        Response = cer.Response,
                        Comment = cer.Comment
                    }).ToArray(),
                    StatusMessages = verifyResult.StatusMessages
                };

                try
                {
                    _responseBus.SendSecretHeartbeatResponse(response);
                    _log.Info(string.Format("Heartbeat Result for Secret Id {0}: {1}", request.SecretId, verifyResult.ErrorCode));
                }
                catch (Exception)
                {
                    _log.Error("Failed to record the secret heartbeat response back to server");
                    //TODO: Retry based on heartbeat status?
                }

            }
            catch (Exception ex)
            {
                _log.Error("Handle specific error here", ex);
                throw;
            }
        }
    }
}
