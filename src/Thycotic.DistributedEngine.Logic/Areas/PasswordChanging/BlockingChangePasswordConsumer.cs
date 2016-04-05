using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.DE.Areas.PasswordChanging.Request;
using Thycotic.Messages.DE.Areas.PasswordChanging.Response;
using Thycotic.PasswordChangers;
using Thycotic.SharedTypes.PasswordChangers;
using Error = Thycotic.SharedTypes.PasswordChangers.Error;

namespace Thycotic.DistributedEngine.Logic.Areas.PasswordChanging
{
    /// <summary>
    /// Secret change password (using account's own credentials) request
    /// </summary>
    public class BlockingChangePasswordConsumer :
        IBlockingConsumer<BlockingPasswordChangeMessage, BlockingPasswordChangeResponse>
    {
        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof (SecretBasicChangePasswordConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        public BlockingChangePasswordConsumer(IResponseBus responseBus)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);

            _responseBus = responseBus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        public BlockingPasswordChangeResponse Consume(CancellationToken token, BlockingPasswordChangeMessage request)
        {
            _log.Info("Got a password change test verify request.");

            try
            {
                BlockingPasswordChangeResponse response;
                var defaultPasswordChangerFactory = new DefaultPasswordChangerFactory();
                var passwordChanger = defaultPasswordChangerFactory.ResolveBasicPasswordChanger(request.OperationInfo);

                if (passwordChanger != null)
                {
                    var info = request.OperationInfo;

                    var changeResult = passwordChanger.ChangeUsingOwnCredentials(info);

                    response = new BlockingPasswordChangeResponse
                    {
                        Success = changeResult.Status == OperationStatus.Success,
                        Errors = changeResult.Errors,
                        CommandExecutionResults = changeResult.CommandExecutionResults
                    };
                }
                else
                {
                    var message = String.Format("No blocking password changer was found for '{0}'.",
                        request.OperationInfo.GetType().Name);

                    _log.Info(message);

                    response = new BlockingPasswordChangeResponse
                    {
                        Success = false,
                        Errors =
                            new List<Error>
                            {
                                new Error(message)
                            },
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                var response = new BlockingPasswordChangeResponse()
                {
                    Success = false,
                    Errors = new List<Error> { new Error(ex.Message, ex.StackTrace) }
                };
                return response;
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