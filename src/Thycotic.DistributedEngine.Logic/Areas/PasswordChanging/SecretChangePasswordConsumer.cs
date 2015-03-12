using System;
using Thycotic.AppCore.Federator;
using Thycotic.DistributedEngine.Logic.Areas.Heartbeat;
using Thycotic.ihawu.Business.Federator;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.Heartbeat.Request;
using Thycotic.Messages.Heartbeat.Response;
using Thycotic.Messages.PasswordChanging.Request;
using Thycotic.Messages.PasswordChanging.Response;

namespace Thycotic.DistributedEngine.Logic.Areas.PasswordChanging
{
    /// <summary>
    /// Secret  heartbeat request
    /// </summary>
    public class SecretChangePasswordConsumer : IBlockingConsumer<SecretChangePasswordMessage, SecretChangePasswordResponse>
    {
        private readonly FederatorProvider _federatorProvider = new FederatorProvider();
        private const string FAILED_VERIFY_MESSAGE = "Could not verify the password change.";
        private const string SUCCESS_MESSAGE = "Password changed successfully.";

        private readonly ILogWriter _log = Log.Get(typeof(SecretHeartbeatConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SecretChangePasswordResponse Consume(SecretChangePasswordMessage request)
        {
            _log.Info("Got a change password request");

            var infoProvider = request.PasswordInfoProvider;
            var passwordTypeName = infoProvider.PasswordTypeName;
            try
            {
                var federator = _federatorProvider.GetFederatorByType(passwordTypeName);
                IChangeResult passwordChangeResult = null;

                if (federator.AllowsChangePasswordByOtherCredential(infoProvider) && infoProvider.PrivilegedAccountInfo != null)
                {
                    passwordChangeResult = federator.SetPasswordByOtherCredentials(infoProvider);
                }
                else
                {
                    passwordChangeResult = federator.SetPassword(infoProvider);
                }
                if (!passwordChangeResult.Success)
                {
                    return new SecretChangePasswordResponse
                    {
                        Success = passwordChangeResult.Success,
                        ErrorCode = passwordChangeResult.ErrorCode,
                        CommandExecutionResults = passwordChangeResult.CommandExecutionResults,
                        StatusMessages = passwordChangeResult.StatusMessages
                    };
                }
                if (!federator.VerifyPassword(infoProvider))
                {
                    return new SecretChangePasswordResponse
                    {
                        Success = false,
                        ErrorCode = FailureCode.UnknownError,
                        StatusMessages = new[] {FAILED_VERIFY_MESSAGE}
                    };
                }
                return new SecretChangePasswordResponse
                {
                    Success = true,
                    ErrorCode = FailureCode.NoError,
                    StatusMessages = new[] { SUCCESS_MESSAGE }
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
