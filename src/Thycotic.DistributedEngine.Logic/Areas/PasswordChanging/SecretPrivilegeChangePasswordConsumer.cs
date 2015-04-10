using System;
using System.Collections.Generic;
using System.Linq;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.General;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.PasswordChanging.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.PasswordChanging.Request;
using Thycotic.PasswordChangers;
using Thycotic.SharedTypes.Federator;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.DistributedEngine.Logic.Areas.PasswordChanging
{
    /// <summary>
    /// Secret change password (using privileged credentials) request
    /// </summary>
    public class SecretPrivilegeChangePasswordConsumer : IBasicConsumer<SecretPrivilegedPasswordChangeMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof(SecretPrivilegeChangePasswordConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        public SecretPrivilegeChangePasswordConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void Consume(SecretPrivilegedPasswordChangeMessage request)
        {
            _log.Info(string.Format("Got a privileged change password request for Secret Id {0}:", request.SecretId));

            try
            {
                RemotePasswordChangeResponse response = null;

                var defaultPasswordChangerFactory = new DefaultPasswordChangerFactory();
                IPrivilegedPasswordChanger passwordChanger = defaultPasswordChangerFactory.ResolvePrivilegedPasswordChangerFromInfo(request.OperationInfo);

                if (passwordChanger != null)
                {
                    var info = request.OperationInfo;

                    var changeResult = passwordChanger.ChangeUsingPrivilegedCredentials(info);

                    if (changeResult.Success)
                    {
                        changeResult = passwordChanger.VerifyNewCredentials(info);
                    }

                    response = new RemotePasswordChangeResponse()
                    {
                        Success = changeResult.Success,
                        SecretId = request.SecretId,
                        ErrorCode = changeResult.Success ? (int) FailureCode.NoError : (int) FailureCode.UnknownError,
                        StatusMessages = changeResult.Errors.Select(e => e.DetailedMessage).ToArray(),
                        CommandExecutionResults = new List<CommandExecutionResult>().ToArray(),
                        OldPassword = string.Empty,
                        NewPassword = info.TargetNewPassword
                    };
                }
                else
                {
                    var message = String.Format("No privileged password changer was found for '{0}'.", request.OperationInfo.GetType().Name);

                    _log.Info(string.Format("{0} Secret Id {1}:", message, request.SecretId));

                    response = new RemotePasswordChangeResponse()
                    {
                        Success = false,
                        SecretId = request.SecretId,
                        ErrorCode = (int) FailureCode.UnknownError,
                        StatusMessages = new[] {message},
                        CommandExecutionResults = new List<CommandExecutionResult>().ToArray(),
                        OldPassword = string.Empty,
                        NewPassword = string.Empty
                    };
                }

                try
                {
                    _responseBus.Execute(response);
                    _log.Info(string.Format("Change Password Result for Secret Id {0}: {1}", request.SecretId, response.ErrorCode));
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