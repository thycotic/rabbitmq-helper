﻿using System;
using System.Collections.Generic;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.Heartbeat.Request;
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

            try
            {
                var verifier =
                    new DefaultPasswordChangerFactory().ResolveCredentialVerifier(request.VerifyCredentialsInfo);
                var verifyResult = verifier.VerifyCredentials(request.VerifyCredentialsInfo);

                var response = new SecretHeartbeatResponse
                {
                    Success = verifyResult.Success,
                    SecretId = request.SecretId,
                    Errors = verifyResult.Errors,
                    Log = verifyResult.Log
                };

                try
                {
                    _responseBus.ExecuteAsync(response);
                    _log.Info(string.Format("Heartbeat Result for Secret Id {0}: Success: {1}", request.SecretId, verifyResult.Success));
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
                    Success = false,
                    SecretId = request.SecretId,
                    Errors = new List<Error> {new ThycoticError(ThycoticErrorType.Unknown, ex.ToString())},
                    Log = new List<LogEntry>()
                };
                _log.Info(string.Format("Heartbeat Result for Secret Id {0}: Success: False ({1})", request.SecretId, ex ));
                _responseBus.ExecuteAsync(failResp);
            }
        }
    }
}
