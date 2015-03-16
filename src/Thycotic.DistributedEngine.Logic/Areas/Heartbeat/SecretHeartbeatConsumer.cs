using System;
using System.Linq;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
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
        private readonly IEngineToServerCommunicationBus _engineToServerCommunicationBus;
        private readonly FederatorProvider _federatorProvider = new FederatorProvider();

        private readonly ILogWriter _log = Log.Get(typeof(SecretHeartbeatConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engineToServerCommunicationBus"></param>
        public SecretHeartbeatConsumer(IEngineToServerCommunicationBus engineToServerCommunicationBus)
        {
            _engineToServerCommunicationBus = engineToServerCommunicationBus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void Consume(SecretHeartbeatMessage request)
        {
            _log.Info("Got a heartbeat request");

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
                    CommandExecutionResults = verifyResult.CommandExecutionResults.Select(cer => new CommandExecutionResult
                    {
                        Command = cer.Command,
                        Response = cer.Response,
                        Comment = cer.Comment
                    }).ToArray(),
                    StatusMessages = verifyResult.StatusMessages
                };

                try
                {
                    _engineToServerCommunicationBus.RecordSecretHeartbeatResponse(response);
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
