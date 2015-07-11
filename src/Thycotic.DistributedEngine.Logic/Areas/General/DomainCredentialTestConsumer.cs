using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.DistributedEngine.Logic.Areas.PasswordChanging;
using Thycotic.Logging;
using Thycotic.Messages.Common;
using Thycotic.Messages.General;
using Thycotic.PasswordChangers.DomainValidation;
using Thycotic.SharedTypes.PasswordChangers;

namespace Thycotic.DistributedEngine.Logic.Areas.General
{
    /// <summary>
    /// Domain credential test consumer
    /// </summary>
    public class DomainCredentialTestConsumer : IBlockingConsumer<DomainCredentialTestMessage, CredentialOperationResult>
    {
        private readonly ILogWriter _log = Log.Get(typeof(DomainCredentialTestConsumer));

        /// <summary>
        /// Consumes a domain credential test message and attempts to authenticate with the given credentials.
        /// </summary>
        /// <param name="request">The request.</param>
        public CredentialOperationResult Consume(DomainCredentialTestMessage request)
        {
            

            _log.Info(string.Format("Got a Domain credential validation request for Domain: {0} using user {1}\\{2}", request.Domain, request.UserDomain, request.UserName));

            var domainVerifier = new DomainValidationVerifier();
            var verifyInfo = new DomainValidationVerifyInfo
            {
                Domain = request.Domain,
                UserPrincipalName = request.UserName,
                UserDomain = request.UserDomain,
                Password = request.Password,
                Port = request.Port,
                UseSSL = request.UseSsl
            };

            var result = domainVerifier.VerifyCredentials(verifyInfo);

            _log.Info(string.Format("Credential validation result for Domain {0} using user {1}\\{2}: {3}", 
                request.Domain, request.UserDomain, request.UserName, 
                result.Status == OperationStatus.Success 
                    ? "Success" 
                    : string.Format("Failure ({0})", string.Join(", ", result.Errors.Select(e => e.Message)))));

            return result;
        }
    }
}
