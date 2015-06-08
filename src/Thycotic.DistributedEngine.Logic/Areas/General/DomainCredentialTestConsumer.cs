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
        /// <summary>
        /// Consumes a domain credential test message and attempts to authenticate with the given credentials.
        /// </summary>
        /// <param name="request">The request.</param>
        public CredentialOperationResult Consume(DomainCredentialTestMessage request)
        {
            var domainVerifier = new DomainValidationVerifier();
            var verifyInfo = new DomainValidationVerifyInfo
            {
                Domain = request.Domain,
                UserPrincipalName = request.UserName,
                Password = request.Password,
                Port = request.Port,
                UseSSL = request.UseSsl
            };

            return domainVerifier.VerifyCredentials(verifyInfo);
        }
    }
}
