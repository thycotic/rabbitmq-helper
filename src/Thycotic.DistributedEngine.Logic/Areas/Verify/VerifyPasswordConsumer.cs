using System;
using System.Collections.Generic;
using System.Threading;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.DE.Areas.Verify.Request;
using Thycotic.Messages.DE.Areas.Verify.Response;
using Thycotic.PasswordChangers;
using Thycotic.SharedTypes.PasswordChangers;
using Error = Thycotic.SharedTypes.PasswordChangers.Error;

namespace Thycotic.DistributedEngine.Logic.Areas.Verify
{
    /// <summary>
    /// Verify Password Consumer
    /// </summary>
    [ConsumerPriority(Priority.Highest)]
    public class VerifyPasswordConsumer : IBlockingConsumer<VerifyPasswordMessage, VerifyPasswordResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(VerifyPasswordConsumer));

        private readonly IResponseBus _responseBus;

         /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        public VerifyPasswordConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }

        /// <summary>
        /// Consumes the request
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="message">VerifyPasswordMessage</param>
        /// <returns>
        /// VerifyPasswordResponse
        /// </returns>
        public VerifyPasswordResponse Consume(CancellationToken token, VerifyPasswordMessage message)
        {
          
            _log.Info(string.Format("Got a verify password request."));

            try
            {
                var verifier = new DefaultPasswordChangerFactory().ResolveCredentialVerifier(message.VerifyCredentialsInfo);
                var verifyResult = verifier.VerifyCredentials(message.VerifyCredentialsInfo);

                var response = new VerifyPasswordResponse
                {
                    Success = verifyResult.Status == OperationStatus.Success,
                    Errors = verifyResult.Errors,
                    CommandExecutionResults = verifyResult.CommandExecutionResults
                };

                return response;

            }
            catch (Exception ex)
            {
                var failResp = new VerifyPasswordResponse
                {
                    Success = false,
                    Errors = new List<Error> { new Error(ex.Message, ex.StackTrace) },
                };
                _log.Info(string.Format("Verify Password Success: False ({0})", ex.Message));
                return failResp;
            }  
        }
    }
}
