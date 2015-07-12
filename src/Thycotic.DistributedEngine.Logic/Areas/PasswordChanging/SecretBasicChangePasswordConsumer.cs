using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Autofac.Features.OwnedInstances;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.PasswordChanging.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.PasswordChanging.Request;
using Thycotic.PasswordChangers;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.DistributedEngine.Logic.Areas.PasswordChanging
{
    /// <summary>
    /// Secret change password (using account's own credentials) request
    /// </summary>
    public class SecretBasicChangePasswordConsumer : IBasicConsumer<SecretBasicPasswordChangeMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly Func<Owned<IBasicConsumer<SecretChangeDependencyMessage>>> _consumerFactory;
        private readonly ILogWriter _log = Log.Get(typeof(SecretBasicChangePasswordConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="consumerFactory"></param>
        public SecretBasicChangePasswordConsumer(IResponseBus responseBus, Func<Owned<IBasicConsumer<SecretChangeDependencyMessage>>> consumerFactory)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(consumerFactory != null);

            _responseBus = responseBus;
            _consumerFactory = consumerFactory;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void Consume(SecretBasicPasswordChangeMessage request)
        {
            _log.Info(string.Format("Got a basic change password request for Secret Id {0}:", request.SecretId));

            try
            {
                RemotePasswordChangeResponse response;
                var runDependencies = false;
                var defaultPasswordChangerFactory = new DefaultPasswordChangerFactory();
                var passwordChanger = defaultPasswordChangerFactory.ResolveBasicPasswordChanger(request.OperationInfo);

                if (passwordChanger != null)
                {
                    var info = request.OperationInfo;

                    var changeResult = passwordChanger.ChangeUsingOwnCredentials(info);

                    if (changeResult.Status == OperationStatus.Success)
                    {
                        changeResult = passwordChanger.VerifyNewCredentials(info);
                    }

                    var allErrors = changeResult.Errors.Select(e => (string.IsNullOrWhiteSpace(e.DetailedMessage) ? e.Message : e.DetailedMessage)).ToArray();

                    response = new RemotePasswordChangeResponse
                    {
                        Status = changeResult.Status,
                        SecretId = request.SecretId,
                        StatusMessages = allErrors,
                        CommandExecutionResults = new List<CommandExecutionResult>().ToArray(),
                        OldPassword = info.CurrentPassword,
                        NewPassword = info.NewPassword
                    };

                    if (changeResult.Status == OperationStatus.Success && request.SecretChangeDependencyMessage != null)
                    {
                        runDependencies = true;
                    }
                }
                else
                {
                    var message = String.Format("No basic password changer was found for '{0}'.", request.OperationInfo.GetType().Name);

                    _log.Info(string.Format("{0} Secret Id {1}:", message, request.SecretId));

                    response = new RemotePasswordChangeResponse
                    {
                        Status = OperationStatus.Unknown,
                        SecretId = request.SecretId,
                        StatusMessages = new[] { message },
                        CommandExecutionResults = new List<CommandExecutionResult>().ToArray(),
                        OldPassword = string.Empty,
                        NewPassword = string.Empty
                    };
                }

                _responseBus.ExecuteAsync(response);
                _log.Info(string.Format("Change Password Result for Secret Id {0}: {1}", request.SecretId, response.Status));

                if (runDependencies)
                {
                    _consumerFactory().Value.Consume(request.SecretChangeDependencyMessage);
                }
            }
            catch (Exception ex)
            {
                var response = new RemotePasswordChangeResponse()
                {
                    Status = OperationStatus.Unknown,
                    SecretId = request.SecretId,
                    StatusMessages = new[] { ex.ToString() },
                    CommandExecutionResults = new List<CommandExecutionResult>().ToArray(),
                    OldPassword = string.Empty,
                    NewPassword = string.Empty
                };
                _responseBus.ExecuteAsync(response);
                _log.Error(string.Format("Change Password Result for Secret Id {0}: {1}", request.SecretId, response.Status));
            }
        }

        /// <summary>
        /// Objects the invariant.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._log != null);
        }
    }
}