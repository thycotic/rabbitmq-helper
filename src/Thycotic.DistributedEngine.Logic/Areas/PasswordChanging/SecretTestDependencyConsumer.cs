using System;
using System.Collections.Generic;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.PasswordChanging.Request;
using Thycotic.Messages.PasswordChanging.Response;
using Thycotic.PasswordChangers.DependencyChangers;

namespace Thycotic.DistributedEngine.Logic.Areas.PasswordChanging
{
    /// <summary>
    /// Secret test dependency request
    /// </summary>
    public class SecretTestDependencyConsumer : IBlockingConsumer<SecretTestDependencyMessage, SecretTestDependencyResponse>
    {
        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof(SecretChangePasswordConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        public SecretTestDependencyConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SecretTestDependencyResponse Consume(SecretTestDependencyMessage request)
        {
            _log.Info(string.Format("Got a Test Dependency request for Secret Id {0} (Dependency Id {1})", request.SecretId, request.DependencyChangeInfo.SecretDependencyId));
            var response = new SecretTestDependencyResponse();
            try
            {
                var changer = new DependencyChangeDispatcher(request.WmiTimeout);
                response.Messages = changer.TestDependencyAction(request.DependencyChangeInfo);
            }
            catch (Exception ex)
            {
                response.Messages = new List<string[]> {new[] {"Error", ex.Message}};
            }
            _log.Info(string.Format("Test Dependency Response for Secret Id {0}.", request.SecretId));
            return response;
        }
    }
}
