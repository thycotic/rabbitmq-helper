using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.DE.Areas.Heartbeat.Request;
using Thycotic.PasswordChangers;
using Thycotic.SharedTypes.Logging;
using Thycotic.SharedTypes.PasswordChangers;
using Error = Thycotic.SharedTypes.PasswordChangers.Error;

namespace Thycotic.DistributedEngine.Logic.Areas.Heartbeat
{
    /// <summary>
    /// Secret  heartbeat request
    /// </summary>
    public class SecretHeartbeatConsumer : IBasicConsumer<SecretHeartbeatMessage>
    {
        private readonly IResponseBus _responseBus;

        private readonly ILogWriter _log = Log.Get(typeof(SecretHeartbeatConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        public SecretHeartbeatConsumer(IResponseBus responseBus)
        {
            Contract.Requires<ArgumentNullException>(responseBus!= null);
            _responseBus = responseBus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        public void Consume(CancellationToken token, SecretHeartbeatMessage request)
        {
            

            _log.Info(string.Format("Got a heartbeat request for Secret Id {0}", request.SecretId));

            try
            {
                var verifier = new DefaultPasswordChangerFactory().ResolveCredentialVerifier(request.VerifyCredentialsInfo);
                var verifyResult = verifier.VerifyCredentials(request.VerifyCredentialsInfo);

                var response = new SecretHeartbeatResponse
                {
                    Status = verifyResult.Status,
                    SecretId = request.SecretId,
                    ApplicationUrl = request.ApplicationUrl,
                    Errors = verifyResult.Errors,
                    Log = verifyResult.Log
                };

                try
                {
                    _responseBus.ExecuteAsync(response);
                    _log.Info(string.Format("Heartbeat Result for Secret Id {0}: Status: {1}", request.SecretId, verifyResult.Status));
                }
                catch (Exception)
                {
                    _log.Error("Failed to record the secret heartbeat response back to server");
                    // May not have to do this anymore.
                }

            }
            catch (Exception ex)
            {
                var failResp = new SecretHeartbeatResponse
                {
                    Status = OperationStatus.Unknown,
                    SecretId = request.SecretId,
                    Errors = new List<Error> {new Error(ex.Message, ex.ToString())},
                    Log = new List<LogEntry>()
                };
                _log.Info(string.Format("Heartbeat Result for Secret Id {0}: Success: False ({1})", request.SecretId, ex ));
                _responseBus.ExecuteAsync(failResp);
            }
        }
    }
}
