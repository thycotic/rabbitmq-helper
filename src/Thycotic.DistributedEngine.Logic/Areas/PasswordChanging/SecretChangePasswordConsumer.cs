using System;
using System.Linq;
using Thycotic.AppCore.Federator;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.PasswordChanging.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.ihawu.Business.Federator;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.PasswordChanging.Request;

namespace Thycotic.DistributedEngine.Logic.Areas.PasswordChanging
{
    /// <summary>
    /// Secret  change password request
    /// </summary>
    public class SecretChangePasswordConsumer : IBasicConsumer<SecretChangePasswordMessage>
    {
        private readonly FederatorProvider _federatorProvider = new FederatorProvider();        

        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof(SecretChangePasswordConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        public SecretChangePasswordConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void Consume(SecretChangePasswordMessage request)
        {
            _log.Info(string.Format("Got a change password request for Secret Id {0}:", request.SecretId));

            var infoProvider = request.PasswordInfoProvider;
            var passwordTypeName = infoProvider.PasswordTypeName;
            try
            {
                var federator = _federatorProvider.GetFederatorByType(passwordTypeName);
                IChangeResult passwordChangeResult;

                if (federator.AllowsChangePasswordByOtherCredential(infoProvider) && infoProvider.PrivilegedAccountInfo != null)
                {
                    passwordChangeResult = federator.SetPasswordByOtherCredentials(infoProvider);
                }
                else
                {
                    passwordChangeResult = federator.SetPassword(infoProvider);
                }

                var response = new RemotePasswordChangeResponse()
                {
                    Success = passwordChangeResult.Success,
                    SecretId = request.SecretId,
                    ErrorCode = (int)passwordChangeResult.ErrorCode,
                    CommandExecutionResults = (passwordChangeResult.CommandExecutionResults ?? new CommandExecutionResult[0]).Select(cer => new EngineToServerCommunication.Areas.General.CommandExecutionResult
                    {
                        Command = cer.Command,
                        Response = cer.Response,
                        Comment = cer.Comment
                    }).ToArray(),
                    StatusMessages = passwordChangeResult.StatusMessages
                };

                try
                {
                    _responseBus.SendRemotePasswordChangeResponse(response);
                    _log.Info(string.Format("Change Password Result for Secret Id {0}: {1}", request.SecretId, passwordChangeResult.ErrorCode));
                }
                catch (Exception)
                {
                    _log.Error("Failed to record the secret change password response back to server");
                    //TODO: Retry?
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
